using JournalAPI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace JournalAPI.Services.Interfaces
{
    public interface IMarkService
    {
        public Task UpdateMarksAsync(List<MarkViewModel> marks);
        public Task ChangeLockedMarksAsync(List<LockedMarkViewModel> marks);
        public Task<List<Mark>> GetMarksForReportAsync(int SemesterId, Locked locked);
        public Task SaveMarksInReportAsync(int SemesterId, Locked locked);
        /// <summary>
        /// Сохранения журнала в Excel
        /// </summary>
        /// <param name="relationId">Id связи</param>
        /// <returns>Возвращается FileContentResult для того чтобы браузер начал загрузку файла</returns>
        public Task<FileContentResult> SaveMarksInExcel(int relationId);
        public Task<FileContentResult> SaveReportInExcel(int SemesterId, Locked locked);
    }
}
