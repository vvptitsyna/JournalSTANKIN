using JournalAPI.Models.RelationModels;
using JournalAPI.Models.SemesterModels;
using JournalAPI.Models.ServicesModels;
using JournalAPI.Models.UserModels;
using JournalAPI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;

namespace JournalAPI.Services.Implementations
{
    public class RelationService : IRelationService
    {
        private readonly AppDbContext _context;

        public RelationService(AppDbContext context)
            => _context = context;

        public async Task AddRelationAsync(RelationViewModel relationView)
        {
            var lecturer = await GetUserById(relationView.LecturerId);

            var teachers = await GetUsersByIds(relationView.TeachersId);

            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Id == relationView.SemesterId);

            if (semester is null)
                throw new Exception($"Не найден семестр с Id {relationView.SemesterId}");

            var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == relationView.SubjectId);

            if (subject is null)
                throw new Exception($"Не найден предмет с Id {relationView.SubjectId}");

            var group = await _context.MainGroups.FirstOrDefaultAsync(g => g.Id == relationView.GroupId);

            if (group is null)
                throw new Exception($"Не группа с Id {relationView.GroupId}");

            var groupWithVersion = _context.MainGroups
                .Include(g => g.GroupsWithVersion)
                    .ThenInclude(g => g.Subgroups)
                        .ThenInclude(s => s.StudentSubgroups)
                .FirstOrDefault(g => g.Id == relationView.GroupId)
                ?.GroupsWithVersion.OrderByDescending(g => g.Version)
                .FirstOrDefault();

            if (groupWithVersion is null)
                throw new Exception($"У группы {group.Name} нет версий");

            var subgroup = groupWithVersion.Subgroups.FirstOrDefault(s => s.Name == relationView.SubgroupName);

            if (subgroup is null)
                throw new Exception($"Ошибка, у группы {groupWithVersion.Name} нет подгруппы с названием {relationView.SubgroupName}");

            Relation relation = new Relation
            {
                HasCoursework = relationView.HasCoursework,
                Semester = semester,
                Subgroup = subgroup,
                Subject = subject,
                Teachers = new List<UserRelation>(),
                SubjectForm = relationView.SubjectForm,
                LecturerName = lecturer.UserName,
                TeachersNames = string.Join(";",teachers.Select(t => t.UserName))
            };

            foreach (var teacher in teachers)
            {
                relation.Teachers.Add(new UserRelation
                {
                    Teacher = teacher,
                    Relation = relation
                });
            }

            var marks = new List<Mark>();

            foreach (var studentSubgroup in subgroup.StudentSubgroups)
            {
                Mark mark = new Mark
                {
                    StudentId = studentSubgroup.StudentId,
                    Relation = relation,
                    Module1 = 0,
                    Module2 = 0,
                    ExamOrTest = 0,
                    Coursework = relation.HasCoursework ? 0 : null
                };

                marks.Add(mark);
            }

            relation.Marks = marks;


            var existRelation = (await GetRelationsAsync())
                .Any(r => r.LecturerName == relation.LecturerName && r.Subgroup == relation.Subgroup && r.Subject == relation.Subject && r.Semester == relation.Semester);

            if (existRelation)
            {
                var fault = $"Связь лектора {relation.LecturerName}, предмета {relation.Subject.Name} и группы {relation.Subgroup.GroupWithVersion.Group.Name} ({relation.Subgroup.Name}) в {relation.Semester.Year}г. {relation.Semester.Season} уже существует {Environment.NewLine}";
                throw new Exception(fault);
            }

            await _context.Relations.AddAsync(relation);
            await _context.SaveChangesAsync();
        }

        public async Task AddRelationsFromFileAsync(IFormFile file)
        {
            var excelRelations = ExcelHelper.GetRelationsFromExcel(file);

            List<Relation> relations = new List<Relation>();

            foreach (var excelRelation in excelRelations)
            {
                List<string> teachernames = new List<string>();
                teachernames.Add(excelRelation.LecturerName);
                teachernames.AddRange(excelRelation.TeacherNames);

                List<User> teachers = await GetUsersByNames(teachernames);

                var groupWithVersion = _context.MainGroups
                    .Include(g => g.GroupsWithVersion)
                        .ThenInclude(g => g.Subgroups)
                            .ThenInclude(s => s.StudentSubgroups)
                    .FirstOrDefault(g => g.Name == excelRelation.GroupName)
                    ?.GroupsWithVersion.OrderByDescending(g => g.Version)
                    .FirstOrDefault();

                if (groupWithVersion is null)
                    throw new Exception($"В системе не найдена группа {excelRelation.GroupName}. Проверьте что у нее есть версии");

                var subgroup = groupWithVersion.Subgroups.FirstOrDefault(s => s.Name == excelRelation.SubgroupName);

                if (subgroup is null)
                {
                    throw new Exception($"Подгруппа {excelRelation.GroupName} ({excelRelation.SubgroupName}) не найдена");
                }

                var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Year == int.Parse(excelRelation.Year) && s.Season == excelRelation.Season);

                if (semester is null)
                {
                    var season = excelRelation.Season == Season.Fall ? "Осень" : "Весна";
                    throw new Exception($"Семестр {excelRelation.Year} {season} не найден");
                }

                var subject = await _context.Subjects.FirstOrDefaultAsync(s => s.Name == excelRelation.SubjectName);

                if (subject is null)
                {
                    throw new Exception($"Предмет {excelRelation.SubjectName} не найден");
                }

                var relation = new Relation
                {
                    HasCoursework = excelRelation.HasCoursework,
                    Semester = semester,
                    Subgroup = subgroup,
                    Teachers = new List<UserRelation>(),
                    Subject = subject,
                    SubjectForm = excelRelation.SubjectForm,
                    TeachersNames = string.Join(";", teachers.Select(t => t.UserName)),
                    LecturerName = excelRelation.LecturerName,
                };

                foreach (var teacher in teachers)
                {
                    relation.Teachers.Add(new UserRelation
                    {
                        Teacher = teacher,
                        Relation = relation
                    });
                }

                var marks = new List<Mark>();

              foreach (var studentSubgroup in subgroup.StudentSubgroups)
              {
                  marks.Add(new Mark
                  {
                      StudentId = studentSubgroup.StudentId,
                      Relation = relation,
                      Module1 = 0,
                      Module2 = 0,
                      ExamOrTest = 0,
                      Coursework = relation.HasCoursework ? 0 : null,
                      Locked = Locked.None
                  });
              }

                relation.Marks = marks;

                var existRelation = (await GetRelationsAsync()).Any(r => r.LecturerName == excelRelation.LecturerName && r.Subgroup == relation.Subgroup && r.Subject == relation.Subject && r.Semester == relation.Semester);

                if (existRelation)
                {
                    var fault = $"Связь лектора {relation.LecturerName}, предмета {relation.Subject.Name} и группы {relation.Subgroup.GroupWithVersion.Group.Name} ({relation.Subgroup.Name}) в {relation.Semester.Year}г. {relation.Semester.Season} уже существует {Environment.NewLine}";
                    throw new Exception(fault);
                }

                relations.Add(relation);
            }

            await _context.Relations.AddRangeAsync(relations);
            await _context.SaveChangesAsync();
        }

        public async Task<List<SubjectInfo>> GetSubjectsInfo(string username, int semesterId)
        {
            var teacher = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            var relations = await _context.Relations
            .Include(r => r.Subject)
            .Include(r => r.Teachers)
            .Where(r => (r.Teachers.Select(t => t.TeacherId).Contains(teacher.Id) || r.LecturerName == teacher.UserName) && r.SemesterId == semesterId)
            .GroupBy(g => g.Subject.Name)
            .Select(r => r.First())
            .ToListAsync();

            List<SubjectInfo> semesterInfos = new List<SubjectInfo>();

            foreach (var relation in relations)
            {
                semesterInfos.Add(new SubjectInfo
                {
                    Comment = relation.Comment,
                    SubjectName = relation.Subject.Name,
                    SubjectId = relation.Subject.Id,
                    LecturerName = relation.LecturerName,
                });
            }

            return semesterInfos;
        }
        public async Task<Relation> GetRelationByIdAsync(int relationId)
        {
            var relation = (await GetRelationsAsync()).FirstOrDefault(r => r.Id == relationId);

            if (relation is null)
                throw new Exception($"Не найдена связь с Id {relationId}");

            return relation;
        }

        public async Task<RelationInfo> GetRelationInfoAsync(int relationId)
        {
            var relation = await GetRelationByIdAsync(relationId);

            var relationInfo = new RelationInfo
            {
                LecturerName = relation.LecturerName,
                TeacherNames = relation.TeachersNames.Split(';').ToList(),
                Comment = relation.Comment,
                Marks = new List<MarkViewModel>()
            };

            foreach (var mark in relation.Marks)
            {
                relationInfo.Marks.Add(new MarkViewModel
                {
                    MarkId = mark.Id,
                    StudentName = mark.Student.Name,
                    Module1 = mark.Module1,
                    Module2 = mark.Module2,
                    ExamOrTest = mark.ExamOrTest,
                    Coursework = mark.Coursework,
                    Locked = mark.Locked
                });
            }

            return relationInfo;
        }

        public async Task<List<AdminRelationInfo>> GetAdminRelationInfosAsync()
        {
            var relations = await GetRelationsAsync();

            List<AdminRelationInfo> relationInfos = new List<AdminRelationInfo>();

            foreach (var relation in relations)
            {
                var seasonName = relation.Semester.Season == Season.Spring ? "Весна" : "Осень";
                var semesterName = $"{relation.Semester.Year}  {seasonName}";
                relationInfos.Add(new AdminRelationInfo
                {
                    Id = relation.Id,
                    SemesterName = semesterName,
                    SubjectName = relation.Subject.Name,
                    SubgroupName = relation.Subgroup.Name,
                    GroupName = relation.Subgroup.GroupWithVersion.Name
                });
            }

            return relationInfos;
        }

        public async Task<List<GroupInfo>> GetGroupsInfo(string username, int semesterId, int subjectId)
        {
            var teacher = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (teacher is null)
                throw new Exception($"Пользователь с ФИО '{username}' не найден");  

            var relations = await _context.Relations
            .Include(r => r.Teachers)
            .Include(r => r.Subgroup)
                    .ThenInclude(s => s.GroupWithVersion)
            .Where(r => (r.Teachers.Select(t => t.TeacherId).Contains(teacher.Id) || r.LecturerName == teacher.UserName) && r.SemesterId == semesterId)
            .Where(r => r.SemesterId == semesterId && r.SubjectId == subjectId)
            .ToListAsync();

            var groupInfos = new List<GroupInfo>();

            foreach (var relation in relations)
            {
                groupInfos.Add(new GroupInfo
                {
                    RelationId = relation.Id,
                    GroupName = relation.Subgroup.GroupWithVersion.Name,
                    SubgroupName = relation.Subgroup.Name,
                });
            }

            return groupInfos;
        }
        

        public async Task<List<Relation>> GetRelationsAsync()
            => await _context.Relations
            .Include(u => u.Subgroup)
                .ThenInclude(s => s.StudentSubgroups)
            .Include(u => u.Subgroup)
                .ThenInclude(s => s.GroupWithVersion)
                    .ThenInclude(g => g.Group)
            .Include(u => u.Teachers)
            .Include(u => u.Subject)
            .Include(u => u.Semester)
            .Include(u => u.Marks)
                .ThenInclude(m => m.Student)
            .OrderByDescending(r => r.Semester.Year).ThenByDescending(r => r.Semester.Season)
            .ToListAsync();

        public async Task<List<Relation>> GetRelationsForSemesterAsync(int semesterId)
            => await _context.Relations
            .Include(r => r.Subject)
            .Include(r => r.Marks)
            .Include(r => r.Subgroup)
                .ThenInclude(s => s.GroupWithVersion)
            .Where(r => r.Semester.Id == semesterId).ToListAsync();

        private async Task<List<User>> GetUsersByIds(List<int> userIds)
        {
            List<User> users = new List<User>();

            foreach (var id in userIds)
            {
                User user = await GetUserById(id);

                users.Add(user);
            }

            return users;
        }
        private async Task<User> GetUserById(int id)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (user is null)
            {
                throw new Exception($"Пользователь с id {id} не найден");
            }

            return user;
        }

        private async Task<List<User>> GetUsersByNames(List<string> usernames)
        {
            List<User> users = new List<User>();

            foreach (var username in usernames)
            {
                User user = await GetUserByName(username);
                
                users.Add(user);
            }

            return users;
        }

        private async Task<User> GetUserByName(string username)
        {
           User user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

           if (user == null)
           {
               throw new Exception($"Пользователь с логином {username} не найден");
           }

            return user;
        }
    }
}
