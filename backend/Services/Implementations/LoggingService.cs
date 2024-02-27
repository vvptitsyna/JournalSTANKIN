using JournalAPI.Models.SemesterModels;
using JournalAPI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace JournalAPI.Services.Implementations
{
    public class LoggingService : ILoggingService
    {
        private readonly AppDbContext _context;

        public LoggingService(AppDbContext context)
            => _context = context;

        public async Task AddLogMessage(LogMessage message)
        {
            message.DateTime = DateTime.Now.ToString("G");

            if (string.IsNullOrEmpty(message.Message))
                throw new Exception($"Не задано сообщение для лога");

            if (string.IsNullOrEmpty(message.UserName))
                throw new Exception($"Не задано имя пользователя для лога");

            await _context.LogMessages.AddAsync(message);
            await _context.SaveChangesAsync();
        }

        public async Task<SaveMarksInExcelInfo> GetSaveMarksInExcelInfo(int relationId)
        {
            SaveMarksInExcelInfo info = new SaveMarksInExcelInfo();

            var relation = await _context.Relations
                .Include(r => r.Subgroup)
                    .ThenInclude(s => s.GroupWithVersion)
                .FirstAsync(r => r.Id == relationId);

            info.GroupName = relation.Subgroup.GroupWithVersion.Name;
            info.SubgroupName = relation.Subgroup.Name;

            return info;
        }

        public async Task<SaveReportInExcelInfo> GetSaveReportInExcelInfo(int semesterId, Locked locked)
        {
            SaveReportInExcelInfo info = new SaveReportInExcelInfo();

            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Id == semesterId);

            if (semester is null)
                throw new Exception($"Логгирование. Не найден семестр с Id {semesterId}.");

            var seasonName = semester.Season == Season.Spring ? "Весна" : "Осень";

            info.SemesterName = $"{semester.Year} {seasonName}";

            info.LockedName = locked switch
            {
                Locked.Module1 => "Модуль 1",
                Locked.Module2 => "Модуль 2",
                Locked.ExamOrTest => "Экзамен или зачет",
                Locked.Coursework => "Курсовая работа",
            };

            return info;
        }

        public async Task<(List<TeacherSaveMarksInfo> marksInfo, string subjectName, string groupName, string subgroupName)> GetTeacherSaveMarksInfo(List<MarkViewModel> marks)
        {
            List<TeacherSaveMarksInfo> info = new List<TeacherSaveMarksInfo>();

            var dbMarks = await _context.Marks
                .Include(m => m.Relation)
                    .ThenInclude(m => m.Subgroup)
                        .ThenInclude(s => s.GroupWithVersion)
                .Include(m => m.Student)
                .Include(m => m.Relation)
                    .ThenInclude(r => r.Subject)
                .Where(m => marks.Select(mark => mark.MarkId)
                .Contains(m.Id)).ToListAsync();

            var firstMark = dbMarks.First();

            var subjectName = firstMark.Relation.Subject.Name;
            var groupName = firstMark.Relation.Subgroup.GroupWithVersion.Name;
            var subroupName = firstMark.Relation.Subgroup.Name;
            var hasCoursework = firstMark.Relation.HasCoursework;

            foreach (var dbMark in dbMarks)
            {
                var newMark = marks.First(m => m.MarkId == dbMark.Id);

                info.Add(new TeacherSaveMarksInfo
                {
                    StudentName = dbMark.Student.Name,
                    Module1 = newMark.Module1 != dbMark.Module1 ? newMark.Module1 : null,
                    Module2 = newMark.Module2 != dbMark.Module2 ? newMark.Module2 : null,
                    ExamOrTest = newMark.ExamOrTest != dbMark.ExamOrTest ? newMark.ExamOrTest : null ,
                    Coursework = newMark.Coursework != dbMark.Coursework && hasCoursework ? newMark.Coursework : null
                });
            }

            return (info, subjectName, groupName,subroupName);
        }

        public async Task<(List<SupportSaveMarksInfo> marksInfo, string subjectName, string groupName, string subgroupName)> GetSupportSaveMarksInfo (List<LockedMarkViewModel> changedMarks)
        {
            List<SupportSaveMarksInfo> info = new List<SupportSaveMarksInfo>();

            var dbMarks = await _context.Marks
                .Include(m => m.Relation)
                    .ThenInclude(m => m.Subgroup)
                        .ThenInclude(s => s.GroupWithVersion)
                .Include(m => m.Student)
                .Include(m => m.Relation)
                    .ThenInclude(r => r.Subject)
                .Where(m => changedMarks.Select(mark => mark.MarkId)
                .Contains(m.Id)).ToListAsync();
            
            var firstMark = dbMarks.First();

            var subjectName = firstMark.Relation.Subject.Name;
            var groupName = firstMark.Relation.Subgroup.GroupWithVersion.Name;
            var subroupName = firstMark.Relation.Subgroup.Name;
            var hasCoursework = firstMark.Relation.HasCoursework;

            foreach (var dbMark in dbMarks)
            {
                var newMark = changedMarks.First(m => m.MarkId == dbMark.Id);

                var supportInfo = new SupportSaveMarksInfo
                {
                    StudentName = dbMark.Student.Name,
                    LockModule1 = null,
                    LockModule2 = null,
                    LockExamOrTest = null,
                    LockCoursework = null,
                };

                if(newMark.Module1Locked != dbMark.Locked.HasFlag(Locked.Module1))
                {
                    if (newMark.Module1Locked == true)
                        supportInfo.LockModule1 = true;
                    else
                        supportInfo.LockModule1 = false;
                }

                if (newMark.Module2Locked != dbMark.Locked.HasFlag(Locked.Module2))
                {
                    if (newMark.Module2Locked == true)
                        supportInfo.LockModule2 = true;
                    else
                        supportInfo.LockModule2 = false;
                }

                if (newMark.ExamOrTestLocked != dbMark.Locked.HasFlag(Locked.ExamOrTest))
                {
                    if (newMark.ExamOrTestLocked == true)
                        supportInfo.LockExamOrTest = true;
                    else
                        supportInfo.LockExamOrTest = false;
                }

                if (newMark.CourseworkLocked != dbMark.Locked.HasFlag(Locked.Coursework) && hasCoursework)
                {
                    if (newMark.CourseworkLocked == true)
                        supportInfo.LockCoursework = true;
                    else
                        supportInfo.LockCoursework = false;
                }

                info.Add(supportInfo);
            }

            return (info, subjectName, groupName, subroupName);
        }

        public async Task<List<SaveReportInfo>> GetSaveReportInfo(int semesterId, Locked locked)
        {
            if (!_context.Semesters.Any(s => s.Id == semesterId))
                throw new Exception($"Не найден семестр с Id {semesterId}");

            var marks = await _context.Marks
                .Include(m => m.Relation)
                    .ThenInclude(r => r.Subgroup)
                        .ThenInclude(s => s.GroupWithVersion)
                .Include(m => m.Relation)
                    .ThenInclude(r => r.Subject)
                .Include(m => m.Student)
                .Where(m => m.Relation.SemesterId == semesterId && !m.Locked.HasFlag(locked)).ToListAsync();

            List<SaveReportInfo> saveReportInfos = new List<SaveReportInfo>();

            foreach (var mark in marks)
            {
                saveReportInfos.Add(new SaveReportInfo
                {
                    StudentName = mark.Student.Name,
                    GroupName = mark.Relation.Subgroup.GroupWithVersion.Name,
                    SubgroupName = mark.Relation.Subgroup.Name,
                    SubjectName = mark.Relation.Subject.Name
                });
            }

            return saveReportInfos;
        }

        public async Task<List<LogMessage>> GetAllLogMessages()
        {
            return await _context.LogMessages.ToListAsync();
        }
    }
}
