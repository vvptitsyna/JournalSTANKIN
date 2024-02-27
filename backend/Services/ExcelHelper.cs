using JournalAPI.Models.MarkModels;
using OfficeOpenXml;

namespace JournalAPI.Services
{
    public class ExcelGroup
    {
        public string Name { get; set; }
        public string Comment { get; set; }
        public List<ExcelSubgroup> Subgroups { get; set; }
        
    }

    public class ExcelSubgroup
    {
        public string Name { get; set; }

        public List<StudentFromExcel> Students { get; set; }
    }

    public class GroupFromExcel
    {
        public string StudentName { get; set; }
        public string StudentComment{ get; set; }
        public string GroupComment { get; set; }
        public string GroupName { get; set; }
        public string SubgroupName { get; set; }  
        public string? LastGroupName { get; set; }  
    }

    public class RelationFromExcel
    {
        public string SubjectName { get; set; }
        public SubjectForm SubjectForm { get; set; }
        public bool HasCoursework { get; set; }
        public string GroupName { get; set; }
        public string SubgroupName { get; set; }
        public string Year { get; set; }
        public Season Season { get; set; }
        public string LecturerName { get; set; }
        public List <string> TeacherNames { get; set; }
        public string Comment { get; set; }
    }

    public class StudentFromExcel
    {
       public string Name { get; set; }
       public string Comment { get; set; }
       public string LastGroupName { get; set; }
       public bool NewStudent { get; set; }
    }

    public class UserFromExcel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public string Comment { get; set; }

    }

    public static class ExcelHelper
    {
        public static List<UserFromExcel> GetUsersFromExcel(IFormFile file)
        {
            var rootFolder = Directory.GetCurrentDirectory();
            var fileName = file.FileName;
            var filePath = Path.Combine(rootFolder, fileName);
            var fileLocation = new FileInfo(filePath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                 file.CopyTo(fileStream);
            }

            var users = new List<UserFromExcel>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileLocation))
            {
                
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0]; //Лист в excel

                int totalRows = workSheet.Dimension.Rows;//Последняя строка на которой есть информация

                for (int i = 2; i <= totalRows; i++)
                {
                    users.Add(new UserFromExcel
                    {
                        UserName = workSheet.Cells[i, 1].Value.ToString(),
                        Password = workSheet.Cells[i, 2].Value.ToString(),
                        Role = GetRoleFromExcel(workSheet.Cells[i, 3].Value.ToString()),
                        Comment = workSheet.Cells[i, 4].Value.ToString()
                    });
                }
            }

            File.Delete(filePath);

            return users;
        }
        public static List<ExcelGroup> GetGroupsFromExcel(IFormFile file)
        {
            var rootFolder = Directory.GetCurrentDirectory();
            var fileName = file.FileName;
            var filePath = Path.Combine(rootFolder, fileName);
            var fileLocation = new FileInfo(filePath);
        
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        
            var excelGroups = new List<GroupFromExcel>();
        
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        
            using (ExcelPackage package = new ExcelPackage(fileLocation))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0]; //Лист в excel
        
                int totalRows = workSheet.Dimension.Rows;//Последняя строка на которой есть информация
        
                for (int i = 2; i <= totalRows; i++)
                {
                    excelGroups.Add(new GroupFromExcel
                    {
                        StudentName = workSheet.Cells[i, 1].Value.ToString(),
                        StudentComment = workSheet.Cells[i, 2].Value?.ToString(),
                        GroupName = workSheet.Cells[i, 3].Value.ToString(),
                        SubgroupName = workSheet.Cells[i, 4].Value.ToString(),
                        GroupComment = workSheet.Cells[i, 5].Value?.ToString(),
                        LastGroupName = workSheet.Cells[i, 6].Value?.ToString(),
                    });
                }
            }
        
            var groupedGroups = excelGroups.GroupBy(g => new {g.GroupName, g.SubgroupName} ).ToList();

            List<ExcelGroup> groups = excelGroups
                .GroupBy(g => g.GroupName)
                .Select(g => new ExcelGroup { Name = g.Key, Subgroups = new List<ExcelSubgroup>() }).ToList();

            foreach (var excelgroup in groupedGroups)
            {
                var group = groups.First(g => g.Name == excelgroup.Key.GroupName);

                var subgroup = new ExcelSubgroup
                {
                    Name = excelgroup.Key.SubgroupName
                };

                var studentFromExcel = excelgroup.Select(g => new StudentFromExcel
                {
                    Name = g.StudentName,
                    Comment = g.StudentComment,
                    LastGroupName = g.LastGroupName,
                    NewStudent = string.IsNullOrEmpty(g.LastGroupName) ? true : false
                }).ToList();

                subgroup.Students = studentFromExcel;

                group.Subgroups.Add(subgroup);
            }
        
            File.Delete(filePath);
        
            return groups;
        }

        public static List<StudentFromExcel> GetStudentsFromExcel(IFormFile file)
        {
            var rootFolder = Directory.GetCurrentDirectory();
            var fileName = file.FileName;
            var filePath = Path.Combine(rootFolder, fileName);
            var fileLocation = new FileInfo(filePath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var students = new List<StudentFromExcel>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileLocation))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0]; //Лист в excel

                int totalRows = workSheet.Dimension.Rows;//Последняя строка на которой есть информация

                for (int i = 2; i <= totalRows; i++)
                {
                    students.Add(new StudentFromExcel
                    {
                        Name = workSheet.Cells[i,1].Value.ToString(),
                        Comment = workSheet.Cells[i,2].Value?.ToString(),
                        NewStudent = workSheet.Cells[i, 3].Value.ToString().ToLower() == "да" ? true : false,
                        LastGroupName = workSheet.Cells[i, 4].Value?.ToString()
                    });
                }
            }

            File.Delete(filePath);

            return students;
        }
        public static List<Subject> GetSubjectsFromExcel(IFormFile file)
        {
            var rootFolder = Directory.GetCurrentDirectory();
            var fileName = file.FileName;
            var filePath = Path.Combine(rootFolder, fileName);
            var fileLocation = new FileInfo(filePath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var subjects = new List<Subject>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileLocation))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0]; //Лист в excel

                int totalRows = workSheet.Dimension.Rows;//Последняя строка на которой есть информация

                for (int i = 2; i <= totalRows; i++)
                {
                    if (workSheet.Cells[i, 1].Value is null)
                        break;

                    subjects.Add(new Subject
                    {
                        Name = workSheet.Cells[i, 1].Value.ToString(),
                        Comment = workSheet.Cells[i, 2].Value?.ToString(),
                    });
                }
            }

            File.Delete(filePath);

            return subjects;
        }
        public static List<RelationFromExcel> GetRelationsFromExcel(IFormFile file)
        {
            var rootFolder = Directory.GetCurrentDirectory();
            var fileName = file.FileName;
            var filePath = Path.Combine(rootFolder, fileName);
            var fileLocation = new FileInfo(filePath);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            var excelRelations = new List<RelationFromExcel>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(fileLocation))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[0]; //Лист в excel

                int totalRows = workSheet.Dimension.Rows;//Последняя строка на которой есть информация

                for (int i = 2; i <= totalRows; i++)
                {
                    string subjectName = workSheet.Cells[i, 1].Value.ToString();
                    bool hasCoursework = workSheet.Cells[i, 4].Value is not null;
                    string groupName = workSheet.Cells[i, 5].Value.ToString();
                    string subgroupName = workSheet.Cells[i, 6].Value.ToString();
                    string year = workSheet.Cells[i, 7].Value.ToString();
                    string lecturerName = workSheet.Cells[i, 10].Value.ToString();
                    string teacherNames = workSheet.Cells[i, 11].Value.ToString();
                    string comment = workSheet.Cells[i, 12].Value.ToString();

                    SubjectForm subjectForm;
                    Season season;

                    if (workSheet.Cells[i, 9].Value is not null && workSheet.Cells[i, 8].Value is not null)
                    {
                        throw new Exception($"Для пользователя {lecturerName} у предмета {subjectName} группы {groupName} ({subgroupName}) задано 2 формы контроля");
                    }
                    else if (workSheet.Cells[i, 9].Value is not null)
                    {
                        season = Season.Spring;
                    }
                    else if (workSheet.Cells[i, 8].Value is not null)
                    {
                        season = Season.Fall;
                    }
                    else
                    {
                        throw new Exception($"У предмета {subjectName} группы {groupName} ({subgroupName}) с лектором {lecturerName} не задана форма контроля");
                    }

                    if (workSheet.Cells[i, 3].Value is not null && workSheet.Cells[i, 2].Value is not null)
                    {
                        throw new Exception($"У предмета {subjectName} группы {groupName} ({subgroupName}) с лектором {lecturerName} задано 2 сезона года");
                    }
                    else if(workSheet.Cells[i, 3].Value is not null)
                    {
                        subjectForm = SubjectForm.Test;
                    }
                    else if(workSheet.Cells[i, 2].Value is not null)
                    {
                        subjectForm = SubjectForm.Exam;
                    }
                    else
                    {
                        throw new Exception($"У предмета {subjectName} группы {groupName} ({subgroupName}) с лектором {lecturerName} не задан сезон года");
                    }

                    excelRelations.Add(new RelationFromExcel
                    {
                        SubjectName = subjectName,
                        SubjectForm = subjectForm,
                        HasCoursework = hasCoursework,
                        GroupName = groupName,
                        SubgroupName = subgroupName,
                        Year = year,
                        Season = season,
                        LecturerName = lecturerName,
                        TeacherNames = teacherNames.Trim(';').Split(';').ToList(),
                        Comment = comment
                    }); 
                }
            }

            File.Delete(filePath);

            return excelRelations;
        }
        public static byte[] WriteMarksInExcel(string filename, Relation relation)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage package = new ExcelPackage();
            
            ExcelWorksheet workSheet = package.Workbook.Worksheets.Add($"Лист 1");

            workSheet.Cells[2, 1].Value = "ФИО";
            workSheet.Cells[2, 2].Value = "Модуль 1";
            workSheet.Cells[2, 3].Value = "Модуль 2";

            ExcelRange range = workSheet.Cells[1, 1, 1, 7];
            range.Merge = true;
            range.Value = filename;


            int i = 3;

            if (relation.HasCoursework)
            {
                workSheet.Cells[2, 4].Value = "Курсовая работа";
                workSheet.Cells[2, 5].Value = relation.SubjectForm == SubjectForm.Exam ? "Экзамен" : "Зачет";

                foreach (var mark in relation.Marks)
                {
                    workSheet.Cells[i, 1].Value = mark.Student.Name;
                    workSheet.Cells[i, 2].Value = mark.Module1;
                    workSheet.Cells[i, 3].Value = mark.Module2;
                    workSheet.Cells[i, 4].Value = mark.Coursework;
                    workSheet.Cells[i, 5].Value = mark.ExamOrTest;

                    i++;
                }
            }
            else
            {
                workSheet.Cells[2, 4].Value = relation.SubjectForm == SubjectForm.Exam ? "Экзамен" : "Зачет";

                foreach (var mark in relation.Marks)
                {
                    workSheet.Cells[i, 1].Value = mark.Student.Name;
                    workSheet.Cells[i, 2].Value = mark.Module1;
                    workSheet.Cells[i, 3].Value = mark.Module2;
                    workSheet.Cells[i, 4].Value = mark.ExamOrTest;
                    i++;
                }
            }

            workSheet.Protection.IsProtected = false;
            workSheet.Protection.AllowSelectLockedCells = false;


            return package.GetAsByteArray();
        }
        public static byte[] WriteReportInExcel(string filename, IEnumerable<Mark> marks, string lockedName, Locked locked)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            ExcelPackage package = new ExcelPackage();

            ExcelWorksheet workSheet = package.Workbook.Worksheets.Add($"Лист 1");

            workSheet.Cells[2, 1].Value = "ФИО";
            workSheet.Cells[2, 2].Value = "Предмет";
            workSheet.Cells[2, 3].Value = "Группа";
            workSheet.Cells[2, 4].Value = "Подгруппа";
            workSheet.Cells[2, 5].Value = lockedName;

            ExcelRange range = workSheet.Cells[1, 1, 1, 7];
            range.Merge = true;
            range.Value = filename;

            int i = 3;
            foreach (var mark in marks)
            {
                workSheet.Cells[i, 1].Value = mark.Student.Name;
                workSheet.Cells[i, 2].Value = mark.Relation.Subject.Name;
                workSheet.Cells[i, 3].Value = mark.Relation.Subgroup.GroupWithVersion.Name;
                workSheet.Cells[i, 4].Value = mark.Relation.Subgroup.Name;

                workSheet.Cells[i, 5].Value = locked switch
                {
                    Locked.Module1 => mark.Module1,
                    Locked.Module2 => mark.Module2,
                    Locked.ExamOrTest => mark.ExamOrTest,
                    Locked.Coursework => mark.Coursework,
                    _ => "Для предмета не выбрана форма контроля"
                };

                i++;
            }

            return package.GetAsByteArray();
        }
        private static string GetRoleFromExcel(string role) => role switch
        {
            "Преподаватель" => "Teacher",
            "Персонал поддержки" => "Support",
            "Администратор" => "Administrator",
        };
    }
}
