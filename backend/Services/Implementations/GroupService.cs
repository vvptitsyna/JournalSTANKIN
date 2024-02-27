using JournalAPI.Models.GroupModels;
using JournalAPI.Models.ServicesModels;
using JournalAPI.Models.StudentModels;
using Microsoft.EntityFrameworkCore;

namespace JournalAPI.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;

        public GroupService(AppDbContext context)
            => _context = context;

        public async Task AddGroupAsync(string name, string comment)
        {
            if (await _context.MainGroups.AnyAsync(g => g.Name == name))
                throw new Exception("Группа с таким названием уже существует");

            MainGroup group = new MainGroup
            {
                Name = name,
                Comment = comment,
            };

            await _context.MainGroups.AddAsync(group);
            await _context.SaveChangesAsync();
        }

        public async Task AddGroupsFromExcel(IFormFile formFile)
        {
            var groupsFromExcel = ExcelHelper.GetGroupsFromExcel(formFile);

            foreach (var excelGroup in groupsFromExcel)
            {
                var mainGroup = await _context.MainGroups
                    .Include(g => g.GroupsWithVersion)
                    .FirstOrDefaultAsync(g => g.Name == excelGroup.Name);

                if (mainGroup is null)
                    throw new Exception($"Группа {excelGroup.Name} не найдена в системе");

                var groupWithVersion = new GroupWithVersion
                {
                    Name = excelGroup.Name,
                    Comment = excelGroup.Comment,
                    DateOfCreateon = DateTime.Now.ToString("G"),
                    Group = mainGroup,
                    Version = mainGroup.GroupsWithVersion.OrderByDescending(g => g.Version).First().Version + 1,
                    Subgroups = new List<Subgroup>()
                };

                foreach (var excelSubgroup in excelGroup.Subgroups)
                {
                    Subgroup subgroup = new Subgroup
                    {
                        Name = excelSubgroup.Name,
                        GroupWithVersion = groupWithVersion,
                        StudentSubgroups = new List<StudentSubgroup>()
                    };

                    foreach (var excelStudent in excelSubgroup.Students)
                    {
                        if (excelStudent.NewStudent)
                        {
                            subgroup.StudentSubgroups.Add(new StudentSubgroup
                            {
                                Student = new Student { Name = excelStudent.Name, Comment = excelStudent.Comment },
                                Subgroup = subgroup
                            });
                        }
                        else
                        {
                            var group = _context.MainGroups
                            .Include(s => s.GroupsWithVersion)
                            .ThenInclude(g => g.Subgroups)
                                .ThenInclude(s => s.StudentSubgroups)
                                    .ThenInclude(s => s.Student)
                            .FirstOrDefault(g => g.Name == excelStudent.LastGroupName);

                            if (group is null)
                                throw new Exception($"В системе не найдена группа {excelStudent.LastGroupName}");

                            var lastVersionOfGroup = group.GroupsWithVersion.OrderByDescending(g => g.Version).First();

                            var allStudents = new List<Student>();

                            lastVersionOfGroup.Subgroups.ForEach(s =>
                            {
                                var students = s.StudentSubgroups.Select(s => s.Student);

                                allStudents.AddRange(students);
                            });

                            var student = allStudents.FirstOrDefault(s => s.Name == excelStudent.Name);

                            if (student is null)
                                throw new Exception($"Студент с ФИО {excelStudent.Name} не найден в системе или не принадлежит последней версии группы {excelStudent.LastGroupName}");

                            if (!string.IsNullOrWhiteSpace(excelStudent.Comment))
                                student.Comment = excelStudent.Comment;

                            subgroup.StudentSubgroups.Add(new StudentSubgroup
                            {
                                Student = student,
                                Subgroup = subgroup
                            });
                        }
                    }

                    groupWithVersion.Subgroups.Add(subgroup);
                }

                mainGroup.GroupsWithVersion.Add(groupWithVersion);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<MainGroup>> GetGroupsAsync()
            => await _context.MainGroups
            .Include(g => g.GroupsWithVersion)
                .ThenInclude(g => g.Subgroups)
            .ToListAsync();

        public async Task<MainGroup> GetGroupAsync(int groupId)
            => await GetGroupById(groupId);

        public async Task<List<GroupWithVersion>> GetGroupVersions(int groupId)
         => (await GetGroupById(groupId)).GroupsWithVersion.OrderByDescending(g => g.Version).ToList();

        public async Task<GroupWithVersion> GetGroupWithVersion(int groupWithVersionId)
         => await GetGroupWithVersionById(groupWithVersionId);

        public async Task<Subgroup> GetSubgroup(int subgroupId)
         => await GetSubgroupByIdAsync(subgroupId);

        public async Task EditGroupWithVersion(int groupWithVersionId, string name, string comment)
        {
            var group = await GetGroupWithVersionById(groupWithVersionId);

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Не задано название группы");

            group.Name = name;
            group.Comment = comment;

            _context.GroupsWithVersion.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task AddSubgroupToGroupWithVersion(int groupWithVersionId, string subgroupName, string comment)
        {
            var group = await GetGroupWithVersionById(groupWithVersionId);

            if (string.IsNullOrWhiteSpace(subgroupName))
                throw new Exception("Не задано название подгруппы");

            if (group.Subgroups.Any(s => s.Name == subgroupName))
                throw new Exception($"Подгруппа с названием {subgroupName} уже принадлежит группе {group.Name}");

            group.Subgroups.Add(new Subgroup
            {
                Name = subgroupName,
                Comment = comment,
            });

            _context.GroupsWithVersion.Update(group);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSubgroupFromGroupWithVersion(int subgroupId)
        {
            var subgroup = await GetSubgroupByIdAsync(subgroupId);
            
            _context.Subgroups.Remove(subgroup);
            await _context.SaveChangesAsync();
        }

        public async Task AddNewStudentToSubgroup(int subgroupId, string name, string comment)
        {
            var subgroup = await GetSubgroupByIdAsync(subgroupId);

            if(subgroup.StudentSubgroups.Select(s => s.Student.Name).Any(studentName => studentName == name ))
                throw new Exception($"Студент с ФИО {name} уже принадлежит подгруппе {subgroup.GroupWithVersion.Name} ({subgroup.Name})");

            var studentSubgroup = new StudentSubgroup
            {
                Student = new Student { Name = name, Comment = comment},
                Subgroup = subgroup
            };

            subgroup.StudentSubgroups.Add(studentSubgroup);

            _context.Subgroups.Update(subgroup);

           await _context.SaveChangesAsync();
        }

        public async Task AddExistingStudentToSubgroup(int subgroupWithVersionId, int studentId)
        {
            var subgroup = await GetSubgroupByIdAsync(subgroupWithVersionId);

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);

            if(student is null)
                throw new Exception($"Студент с Id {studentId} не найден в системе");

            if (StudentIsInSubgroup(subgroup, student))
                throw new Exception($"Студент {student.Name} уже принадлежит подгруппе {subgroup.GroupWithVersion.Name} ({subgroup.Name})");

            var studentSubgroup = new StudentSubgroup
            {
                Student = student,
                Subgroup = subgroup
            };

            subgroup.StudentSubgroups.Add(studentSubgroup);

            _context.Subgroups.Update(subgroup);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteStudentFromSubgroup(int subgroupWithVersionId, int studentId)
        {
            var subgroup = await GetSubgroupByIdAsync(subgroupWithVersionId);

            var student = await _context.Students.FirstOrDefaultAsync(s => s.Id == studentId);

            if (student is null)
                throw new Exception($"Студент с Id {studentId} не найден в системе");

            var studentSubgroup =  subgroup.StudentSubgroups.FirstOrDefault(s => s.Subgroup == subgroup && s.Student == student);

            if (studentSubgroup is null)
                throw new Exception($"Студент {student.Name} не принадлежит подгруппе {subgroup.GroupWithVersion.Name} ({subgroup.Name})");

            subgroup.StudentSubgroups.Remove(studentSubgroup);

            _context.Subgroups.Update(subgroup);
            await _context.SaveChangesAsync();
        }

        public async Task CreateNewVersionOfGroup(int groupId)
        {
            var lastGroupWithVersion = (await GetGroupVersions(groupId)).OrderByDescending(g => g.Version).FirstOrDefault();

            GroupWithVersion newGroupWithVersion = null;

            if (lastGroupWithVersion is null)
            {
                newGroupWithVersion = new GroupWithVersion
                {
                    GroupId = groupId,
                    Name = (await GetGroupById(groupId)).Name,
                    Version = 1,
                    DateOfCreateon = DateTime.Now.ToString("G")
                };
            }
            else
            {
                newGroupWithVersion = (GroupWithVersion)lastGroupWithVersion.Clone();
            }

            await _context.GroupsWithVersion.AddAsync(newGroupWithVersion);
            await _context.SaveChangesAsync();
        }

        public async Task AddStudentsToSubgroupFromFile(int subgroupWithVersionId, IFormFile formFile)
        {
            var studentsFromExcel = ExcelHelper.GetStudentsFromExcel(formFile);

            var subgroup = await GetSubgroupByIdAsync(subgroupWithVersionId);

            List<Student> students = new List<Student>();

            foreach (var studentFromExcel in studentsFromExcel)
            {
                if(studentFromExcel.NewStudent)
                {
                    var student = new Student
                    {
                        Name = studentFromExcel.Name,
                        Comment = studentFromExcel.Comment,
                        StudentSubgroups = new List<StudentSubgroup>()
                    };

                    students.Add(student);
                }
                else
                {
                    var group = _context.MainGroups
                        .Include(s => s.GroupsWithVersion)
                            .ThenInclude(g => g.Subgroups)
                                .ThenInclude(s => s.StudentSubgroups)
                                    .ThenInclude(s => s.Student)
                        .FirstOrDefault(g => g.Name == studentFromExcel.LastGroupName);

                    if(group is null)
                        throw new Exception($"В системе не найдена группа {studentFromExcel.LastGroupName}");

                    var lastVersionOfGroup = group.GroupsWithVersion.OrderByDescending(g => g.Version).First();
                    
                    var allStudents = new List<Student>();

                    lastVersionOfGroup.Subgroups.ForEach(s =>
                    {
                        var students = s.StudentSubgroups.Select(s => s.Student);

                        allStudents.AddRange(students);
                    });

                    var student = allStudents.FirstOrDefault(s => s.Name == studentFromExcel.Name);

                    if (student is null)
                        throw new Exception($"Студент с ФИО {studentFromExcel.Name} не найден в системе или не принадлежит последней версии группы {studentFromExcel.LastGroupName}");

                    if(subgroup.StudentSubgroups.Select(s => s.Student).Contains(student))
                        throw new Exception($"Студент с ФИО {studentFromExcel.Name} уже принадлежит данной подгруппе");

                    student.Comment = studentFromExcel.Comment;

                    students.Add(student);
                }
            }

            var existingStudentsNames = subgroup.StudentSubgroups.Select(s => s.Student.Name);

            foreach (var student in students)
            {
                if(existingStudentsNames.Contains(student.Name))
                    throw new Exception($"Студент с ФИО {student.Name} уже принадлежит данной подгруппе");

                subgroup.StudentSubgroups.Add(new StudentSubgroup
                {
                    Student = student,
                    Subgroup = subgroup
                });
            }


            _context.Subgroups.Update(subgroup);
            await _context.SaveChangesAsync();
        }

        public async Task<List<GroupWithVersionInfo>> GetGroupVersionsInfo(int groupId)
        {
            var mainGroup = await GetGroupAsync(groupId);

            List<GroupWithVersionInfo> groupWithVersionInfos = new List<GroupWithVersionInfo>();

            foreach (var groupWithVersion in mainGroup.GroupsWithVersion)
            {
                groupWithVersionInfos.Add(await GetGroupWithVersionInfo(groupWithVersion.Id));
            }

            return groupWithVersionInfos;
        }
        public async Task<GroupWithVersionInfo> GetGroupWithVersionInfo(int groupWithVersionId)
        {
            var groupWithVersion = await GetGroupWithVersionById(groupWithVersionId);

            var subgroupsInfo = new List<SubgroupInfo>();

            foreach (var subgroup in groupWithVersion.Subgroups)
            {
                subgroupsInfo.Add(await GetSubgroupInfo(subgroup.Id));
            }

            var info = new GroupWithVersionInfo
            {
                Id = groupWithVersion.Id,
                DateOfCreateon = groupWithVersion.DateOfCreateon,
                Comment = groupWithVersion.Comment,
                Version = groupWithVersion.Version,
                Name = groupWithVersion.Name,
                MainGroupName = groupWithVersion.Group.Name,
                Subgroups = subgroupsInfo
            };

            return info;
        }

        public async Task<SubgroupInfo> GetSubgroupInfo(int subgroupId)
        {
            var subgroup = await GetSubgroupByIdAsync(subgroupId);

            var subgroupInfo = new SubgroupInfo
            {
                Id = subgroup.Id,
                Name = subgroup.Name,
                Comment = subgroup.Comment,
                GroupWithVersionName = subgroup.GroupWithVersion.Name,
                MainGroupName = subgroup.GroupWithVersion.Group.Name,
                Version = subgroup.GroupWithVersion.Version,
                Students = new List<StudentInfo>()
            };

            foreach (var student in subgroup.StudentSubgroups.Select(s => s.Student))
            {
                subgroupInfo.Students.Add(new StudentInfo
                {
                    Id = student.Id,
                    Name = student.Name,
                    Comment = student.Comment
                });
            }

            return subgroupInfo;
        }

        private bool StudentIsInSubgroup(Subgroup subgroup, Student student)
            => subgroup.StudentSubgroups.Select(s => s.Student).Contains(student);

        private async Task<MainGroup> GetGroupById(int groupId)
        {
            var group = await _context.MainGroups
            .Include(g => g.GroupsWithVersion)
                .ThenInclude(g => g.Subgroups)
                    .ThenInclude(s => s.StudentSubgroups)
            .FirstOrDefaultAsync(g => g.Id == groupId);

            if (group is null)
                throw new Exception($"Группа с Id {groupId} не найдена");
            
            return group;
        }
         
        private async Task<GroupWithVersion> GetGroupWithVersionById(int groupWithVersionId)
        {
            var groupWithVersion =  await _context.GroupsWithVersion
              .Include(g => g.Subgroups)
              .Include(g => g.Group)
              .FirstOrDefaultAsync(g => g.Id == groupWithVersionId);

            if (groupWithVersion is null)
                throw new Exception($"Группа с версией с Id {groupWithVersionId} не найдена");

            return groupWithVersion;
        }
         
        private async Task<Subgroup> GetSubgroupByIdAsync(int subgroupId)
        {
            var subgroup  = await _context.Subgroups
            .Include(s => s.GroupWithVersion)
                .ThenInclude(g => g.Group)
            .Include(s => s.StudentSubgroups)
                .ThenInclude(s => s.Student)
            .FirstOrDefaultAsync(s => s.Id == subgroupId);

            if (subgroup is null)
                throw new Exception($"Подгруппа с Id {subgroupId} не найдена");

            return subgroup;
        }

        private async Task<GroupWithVersion> GetLastGroupWithVersion(int groupId)
            => (await GetGroupVersions(groupId)).OrderByDescending(g => g.Version).First();
        public async Task<List<string>> GetSubgroupsNames(int groupId)
        {
            var group = await GetLastGroupWithVersion(groupId);

            List<string> subgroups = new List<string>();

            foreach (var subgroup in group.Subgroups)
            {
                subgroups.Add(subgroup.Name);
            }

            return subgroups;
        }
    }
}
