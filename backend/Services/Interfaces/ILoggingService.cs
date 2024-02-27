using JournalAPI.Models.Logging;
using JournalAPI.Models.ViewModels;

namespace JournalAPI.Services.Interfaces
{
    public interface ILoggingService
    {
        Task AddLogMessage(LogMessage message);
        Task<SaveMarksInExcelInfo> GetSaveMarksInExcelInfo(int relationId);
        Task<SaveReportInExcelInfo> GetSaveReportInExcelInfo(int semesterId, Locked locked);
        Task<(List<TeacherSaveMarksInfo> marksInfo, string subjectName, string groupName, string subgroupName)> GetTeacherSaveMarksInfo(List<MarkViewModel> marks);
        Task<(List<SupportSaveMarksInfo> marksInfo, string subjectName, string groupName, string subgroupName)> GetSupportSaveMarksInfo(List<LockedMarkViewModel> changedMarks);
        Task<List<SaveReportInfo>> GetSaveReportInfo(int semesterId, Locked locked);
        Task<List<LogMessage>> GetAllLogMessages();
    }
}
