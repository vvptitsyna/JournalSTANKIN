using JournalAPI.Models.RelationModels;
using JournalAPI.Models.ServicesModels;
using JournalAPI.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace JournalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Teacher")]
    public class TeacherController : ControllerBase
    {
        private readonly ISemesterService _semesterService;
        private readonly IRelationService _relationService;
        private readonly IMarkService _markService;
        private readonly ILoggingService _loggingService;

        public TeacherController(ISemesterService semesterService, IRelationService relationService, IMarkService markService, ILoggingService loggingService)
        {
            _semesterService = semesterService;
            _relationService = relationService;
            _markService = markService;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Получение списка семестров преподавателя. Вызывается при заходе на главную страницу преподавателя.
        /// </summary>
        /// <returns>Список семестров в которых у преподавателя есть связи</returns>
        [HttpGet("getTeacherSemesters")]
        public async Task<List<Semester>> GetTeacherSemesters()
        {
            var semesters = await _semesterService.GetSemestersForUserAsync(User.Identity.Name);

            return semesters;
        }

        /// <summary>
        /// Получение предметов преподавателя в семестре. 
        /// </summary>
        [HttpGet("getSemesterInfo")]
        public async Task<List<SubjectInfo>> GetSemesterInfo(int semesterId)
        {
            var semesterInfos = (await _relationService.GetSubjectsInfo(User.Identity.Name, semesterId));

            return semesterInfos;
        }

        /// <summary>
        /// Получение групп в семестре для преподавателя.
        /// </summary>
        [HttpGet("getGroupsInfo")]
        public async Task<List<GroupInfo>> GetGroupsInfo(int semesterId, int subjectId)
        {
            var groupInfos = await _relationService.GetGroupsInfo(User.Identity.Name, semesterId, subjectId);

            return groupInfos;
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
        /// Сохранение журнала в файл excel
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
            finally
            {
                await _loggingService.AddLogMessage(logMessage);
            }

            if (logMessage.Success)
                return file;
            else
                throw new Exception(logMessage.ExceptionMessage);
        }

        /// <summary>
        /// Сохранение оценок в журнале
        /// </summary>
        [HttpPost("saveMarks")]
        public async Task SaveMarks([FromBody]List<MarkViewModel> changedMarks)
        {
            var infos = await _loggingService.GetTeacherSaveMarksInfo(changedMarks);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Сохранение оценок в журнале. Группа: {infos.groupName}. Подгруппа {infos.subgroupName}. Предмет {infos.subjectName}.");

            foreach (var info in infos.marksInfo)
            {
                if(info.Module1.HasValue || info.Module2.HasValue || info.ExamOrTest.HasValue || info.Coursework.HasValue)
                    sb.AppendLine($"Студент: {info.StudentName}. ");

                if (info.Module1.HasValue)
                    sb.Append($"М1:{info.Module1.Value}. ");

                if (info.Module2.HasValue)
                    sb.Append($"М2:{info.Module2.Value}. ");

                if (info.ExamOrTest.HasValue)
                    sb.Append($"ЭКЗ/ЗАЧ:{info.ExamOrTest.Value}. ");

                if (info.Coursework.HasValue)
                    sb.Append($"КР:{info.Coursework.Value}. ");
            }
            
            var logMessage = new LogMessage
            {
                Action = UserAction.SaveInExcel,
                UserName = User.Identity.Name,
                Message = sb.ToString(),
            };

            try
            {
                await _markService.UpdateMarksAsync(changedMarks);
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
    }
}
