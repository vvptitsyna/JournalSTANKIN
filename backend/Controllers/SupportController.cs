using JournalAPI.Models.Logging;
using JournalAPI.Models.SemesterModels;
using JournalAPI.Models.ServicesModels;
using JournalAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace JournalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Support")]
    public class SupportController : ControllerBase
    {
        private readonly IRelationService _relationService;
        private readonly IMarkService _markService;
        private readonly ISemesterService _semesterService;
        private readonly ILoggingService _loggingService;

        public SupportController(IRelationService relationService, IMarkService markService, ISemesterService semesterService, ILoggingService loggingService)
        {
            _relationService = relationService;
            _markService = markService;
            _semesterService = semesterService;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Получение списка всех семестров.
        /// </summary>
        /// <returns>Список всех семестров</returns>
        [HttpGet("getSemesters")]
        public async Task<List<Semester>> GetSemesters()
        {
            var semesters = (await _semesterService.GetSemestersAsync()).OrderByDescending(s => s.Year).ThenByDescending(s => s.Season).ToList();

            return semesters;
        }

        /// <summary>
        /// Получение информации о семестре
        /// </summary>
        /// <returns>Список связей в семестре</returns>
        [HttpGet("getSemesterInfo")]
        public async Task<List<Relation>> GetSemesterInfo(int semesterId)
        {
            var relations = (await _relationService.GetRelationsForSemesterAsync(semesterId));

            return relations;
        }

        /// <summary>
        /// Получение информации о связи.
        /// </summary>
        /// <returns>Связь</returns>
        [HttpGet("getRelationInfo")]
        public async Task<RelationInfo> GetRelationInfo(int relationId)
        {
            var relationInfo = await _relationService.GetRelationInfoAsync(relationId);

            return relationInfo;
        }

        /// <summary>
        /// Сохранение журнала в excel
        /// </summary>
        /// <returns>Файл excel</returns>
        [HttpGet("saveMarksInExcel")]
        public async Task<FileContentResult> SaveMarksInExcel(int relationId)
        {
            FileContentResult file = null;

            var info = await _loggingService.GetSaveMarksInExcelInfo(relationId);

            var logMessage = new LogMessage
            {
                Action = UserAction.SaveInExcel,
                UserName = User.Identity.Name,
                Message = $"Сохранение журнала в Excel. Группа {info.GroupName}. Подгруппа {info.SubgroupName}"
            };

            try
            {
                file = await _markService.SaveMarksInExcel(relationId);
                logMessage.Success = true;
            }
            catch (Exception ex)
            {
                logMessage.Success = false;
                logMessage.ExceptionMessage = ex.Message;
            }

            await _loggingService.AddLogMessage(logMessage);

            if (logMessage.Success)
                return file;
            else
                throw new Exception(logMessage.ExceptionMessage);
        }

        /// <summary>
        /// Сохранение оценок в журнале
        /// </summary>
        [HttpPost("saveMarks")]
        public async Task SaveMarks([FromBody] List<LockedMarkViewModel> changedMarks)
        {
            var infos = await _loggingService.GetSupportSaveMarksInfo(changedMarks);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Проставление оценок в журнале. Группа: {infos.groupName}. Подгруппа {infos.subgroupName}. Предмет {infos.subjectName}.");

            foreach (var info in infos.marksInfo)
            {
                if (info.LockModule1.HasValue || info.LockModule2.HasValue || info.LockExamOrTest.HasValue || info.LockCoursework.HasValue)

                    sb.AppendLine($"Студент: {info.StudentName}. ");

                if (info.LockModule1.HasValue)
                {
                    if(info.LockModule1.Value)
                        sb.Append($"Проставлен М1. ");
                    else
                        sb.Append($"Разблокирован М1. ");
                }

                if (info.LockModule2.HasValue)
                {
                    if (info.LockModule2.Value)
                        sb.Append($"Проставлен М2. ");
                    else
                        sb.Append($"Разблокирован М2. ");
                }

                if (info.LockExamOrTest.HasValue)
                {
                    if (info.LockExamOrTest.Value)
                        sb.Append($"Проставлен ЭКЗ/ЗАЧ. ");
                    else
                        sb.Append($"Разблокирован ЭКЗ/ЗАЧ. ");
                }

                if (info.LockCoursework.HasValue)
                {
                    if (info.LockCoursework.Value)
                        sb.Append($"Проставлена КР. ");
                    else
                        sb.Append($"Разблокирована КР. ");
                }
            }

            var logMessage = new LogMessage
            {
                Action = UserAction.SaveMarks,
                UserName = User.Identity.Name,
                Message = sb.ToString(),
            };

            try
            {
                await _markService.ChangeLockedMarksAsync(changedMarks);
                logMessage.Success = true;
            }
            catch (Exception ex)
            {
                logMessage.Success = false;
                logMessage.ExceptionMessage = ex.Message;
                throw new Exception(logMessage.ExceptionMessage);
            }
            finally
            {
                await _loggingService.AddLogMessage(logMessage);
            }
        }

        /// <summary>
        /// Получение списка оценок для отчета
        /// </summary>
        /// <param name="semesterId">Id семестра</param>
        /// <param name="locked">Форма контроля</param>
        /// <returns>Список оценок</returns>
        [HttpGet("getMarksForReport")]
        public async Task<List<Mark>> GetMarksForReport(int semesterId, Locked locked)
        {
            var marks = await _markService.GetMarksForReportAsync(semesterId, locked);

            return marks;
        }

        /// <summary>
        /// Проставление оценок в отчете
        /// </summary>
        /// <param name="semesterId">Id семестра</param>
        /// <param name="locked">Форма контроля</param>
        [HttpGet("saveMarksInReport")]
        public async Task SaveMarksInReport(int semesterId, Locked locked)
        {
            var infos = await _loggingService.GetSaveReportInfo(semesterId, locked);

            StringBuilder sb = new StringBuilder();

            var lockedName = locked switch
            {
               Locked.Module1 => "Модуль 1",  
               Locked.Module2 => "Модуль 2",  
               Locked.ExamOrTest => "Экзамен или зачет",  
               Locked.Coursework => "Курсовая работа",  
            };

            sb.AppendLine($"Проставление оценок в отчете. {lockedName}.");

            foreach (var info in infos)
            {
                sb.Append($"Студент: {info.StudentName}. Предмет: {info.SubjectName}. Группа: {info.GroupName}. Подгруппа: {info.SubgroupName}.");
            }

            var logMessage = new LogMessage
            {
                Action = UserAction.SaveReport,
                UserName = User.Identity.Name,
                Message = sb.ToString(),
            };

            try
            {
                await _markService.SaveMarksInReportAsync(semesterId, locked);
                logMessage.Success = true;
            }
            catch (Exception ex)
            {
                logMessage.Success = false;
                logMessage.ExceptionMessage = ex.Message;
                throw new Exception(logMessage.ExceptionMessage);
            }
            finally
            {
                await _loggingService.AddLogMessage(logMessage);
            }
        }

        /// <summary>
        /// Сохранение отчета в файл Excel
        /// </summary>
        /// <param name="semesterId">Id семестра</param>
        /// <param name="locked">Форма контроля</param>
        [HttpGet("saveReportInExcel")]
        public async Task<FileContentResult> SaveReportInExcel(int semesterId, Locked locked)
        {
            var info = await _loggingService.GetSaveReportInExcelInfo(semesterId, locked);

            LogMessage logMessage = new LogMessage
            {
                Action = UserAction.SaveInExcel,
                UserName = User.Identity.Name,
                Message = $"Сохранение отчета в Excel файл. Семестр:{info.SemesterName} {info.LockedName}"
            };

            FileContentResult file = null;

            try
            {
              file = await _markService.SaveReportInExcel(semesterId, locked);
              logMessage.Success = true;
            }
            catch (Exception ex)
            {
                logMessage.Success = false;
                logMessage.ExceptionMessage = ex.Message;
            }
            finally
            {
                _loggingService.AddLogMessage(logMessage);
            }

            if (logMessage.Success)
                return file;
            else
                throw new Exception(logMessage.ExceptionMessage);
        }
    }
}
