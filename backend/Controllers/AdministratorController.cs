using JournalAPI.Models.ServicesModels;
using JournalAPI.Models.ViewModels;
using JournalAPI.Models.ViewModels.Group;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JournalAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : ControllerBase
    {
        private readonly IGroupService _groupService;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly IRelationService _relationService;
        private readonly ISubjectService _subjectService;
        private readonly ISemesterService _semesterService;
        private readonly ILoggingService _loggingService;

        public AdministratorController(IGroupService groupService, IStudentService studentService, IUserService userService, IRelationService relationService, ISubjectService subjectService, ISemesterService semesterService, ILoggingService loggingService)
        {
            _groupService = groupService;
            _studentService = studentService;
            _userService = userService;
            _relationService = relationService;
            _subjectService = subjectService;
            _semesterService = semesterService;
            _loggingService = loggingService;
        }


        #region Управление предметами
        /// <summary>
        /// Получение всех предметов
        /// </summary>
        /// <returns>Список предметов</returns>
        [HttpGet("Subject/getSubjects")]
        public async Task<List<Subject>> GetSubjects()
        {
            var subjects = await _subjectService.GetSubjectsAsync();

            return subjects;
        }

        /// <summary>
        /// Добавление предмета в систему
        /// </summary>
        [HttpPost("Subject/addSubject")]
        public async Task AddSubject([FromBody] SubjectViewModel model)
        {
            await _subjectService.AddSubjectAsync(model.Name, model.Comment);
        }

        /// <summary>
        /// Добавление предметов из файла Excel
        /// </summary>
        [HttpPost("Subject/addSubjectsFromExcel")]
        public async Task AddSubjectsFromExcel([FromForm] ExcelFileViewModel model)
        {
            await _subjectService.AddSubjectsFromFileAsync(model.ExcelFile);
        }
        #endregion

        #region Управление семестрами
        /// <summary>
        /// Получение всех семестров
        /// </summary>
        /// <returns>Список семестров</returns>
        [HttpGet("Semester/getSemesters")]
        public async Task<List<Semester>> GetSemesters()
        {
            var semesters = await _semesterService.GetSemestersAsync();

            return semesters;
        }

        /// <summary>
        /// Добавление семестра в систему
        /// </summary>
        [HttpPost("Semester/addSemester")]
        public async Task AddSemester([FromBody] SemesterViewModel model)
        {
            await _semesterService.AddSemesterAsync(model.Year, model.Season);
        }

        /// <summary>
        /// Создание нескольких семестров
        /// </summary>
        [HttpPost("Semester/generateSemesters")]
        public async Task GenerateSemesters([FromBody] GenerateSemesterViewModel model)
        {
            await _semesterService.GenerateSemestersAsync(model.YearStart, model.YearEnd);
        }
        #endregion

        #region Управление пользователями
        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <returns>Список пользователей</returns>
        [HttpGet("User/getUsers")]
        public async Task<List<User>> GetUsers()
        {
            var users = await _userService.GetUsersAsync();

            return users;
        }

        /// <summary>
        /// Получение информации о пользователях
        /// </summary>
        [HttpGet("User/getUsersInfo")]
        public async Task<List<UserViewModel>> GetUsersInfo()
        {
            var userInfos = await _userService.GetUsersInfo();

            return userInfos;
        }

        /// <summary>
        /// Получение информации о пользователе
        /// </summary>
        [HttpGet("User/getUserInfo")]
        public async Task<UserViewModel> GetUserInfo(int userId)
        {
            var userInfo = (await _userService.GetUsersInfo()).First(u => u.Id == userId);

            return userInfo;
        }

        /// <summary>
        /// Добавление пользователя в систему
        /// </summary>
        [HttpPost("User/addUser")]
        public async Task AddUser([FromBody] UserViewModel model)
        {
            await _userService.AddUserAsync(model.UserName, model.Password, model.Role, model.Comment);
        }

        /// <summary>
        /// Добавление пользователей из excel
        /// </summary>
        [HttpPost("User/addUsersFromExcel")]
        public async Task AddUsersFromExcel([FromForm] ExcelFileViewModel model)
        {
            await _userService.AddUsersFromFileAsync(model.ExcelFile);
        }

        /// <summary>
        /// Изменение данных пользователя
        /// </summary>
        [HttpPost("User/editUser")]
        public async Task EditUser([FromBody] UserViewModel model)
        {
            await _userService.UpdateUserAsync(model);
        }
        #endregion

        #region Управление группами
        /// <summary>
        /// Получение всех групп
        /// </summary>
        /// <returns>Список групп</returns>
        [HttpGet("Group/getGroups")]
        public async Task<List<MainGroup>> GetGroups()
        {
            var groups = await _groupService.GetGroupsAsync();

            return groups;
        }

        /// <summary>
        /// Добавление группы в систему
        /// </summary>
        [HttpPost("Group/addGroup")]
        public async Task AddGroup([FromBody] MainGroupModel model)
        {
            await _groupService.AddGroupAsync(model.Name, model.Comment);
        }

        /// <summary>
        /// Добавление групп в систему из файла Excel
        /// </summary>
        [HttpPost("Group/addGroupsFromExcel")]
        public async Task AddGroupsFromExcel([FromForm] ExcelFileViewModel model)
        {
            await _groupService.AddGroupsFromExcel(model.ExcelFile);
        }

        /// <summary>
        /// Получение версиий группы
        /// </summary>
        /// <returns>Список групп с версиями</returns>
        [HttpGet("Group/getGroupVersions")]
        public async Task<List<GroupWithVersionInfo>> GetGroupVersions(int groupId)
        {
            var groupsWithVersion = await _groupService.GetGroupVersionsInfo(groupId);

            return groupsWithVersion;
        }

        /// <summary>
        /// Создание новой версии группы на основе предыдущей
        /// </summary>
        [HttpGet("Group/createNewVersionOfGroup")]
        public async Task CreateNewVersionOfGroup(int groupId)
        {
            await _groupService.CreateNewVersionOfGroup(groupId);
        }

        /// <summary>
        /// Получение информации о версии группы
        /// </summary>
        /// <returns>Группа с версией</returns>
        [HttpGet("Group/getGroupWithVersionInfo")]
        public async Task<GroupWithVersionInfo> GetGroupWithVersionInfo(int groupWithVersionId)
        {
            var groupWithVersion = await _groupService.GetGroupWithVersionInfo(groupWithVersionId);

            return groupWithVersion;
        }

        /// <summary>
        /// Изменение группы с версией
        /// </summary>
        [HttpPost("Group/editGroupWithVersion")]
        public async Task EditGroupWithVersion([FromBody] GroupWithVersionViewModel model)
        {
            await _groupService.EditGroupWithVersion(model.GroupWithVersionId, model.Name, model.Comment);
        }

        /// <summary>
        /// Добавление подгруппы в группу с версией
        /// </summary>
        [HttpPost("Group/addSubgroup")]
        public async Task AddSubgroup([FromBody] SubgroupViewModel model)
        {
            await _groupService.AddSubgroupToGroupWithVersion(model.GroupWithVersionId, model.Name, model.Comment);
        }

        /// <summary>
        /// Добавление подгруппы в группу с версией
        /// </summary>
        [HttpGet("Group/addSubgroup")]
        public async Task DeleteSubgroup(int subgroupId)
        {
            await _groupService.DeleteSubgroupFromGroupWithVersion(subgroupId);
        }

        /// <summary>
        /// Получение всех студентов
        /// </summary>
        /// <returns>Список студентов</returns>
        [HttpGet("Group/getAllStudents")]
        public async Task<List<StudentInfo>> GetAllStudents()
        {
            var students = await _studentService.GetAllStudents();

            return students;
        }

        /// <summary>
        /// Получение информации о подгруппе
        /// </summary>
        [HttpGet("Group/getSubgroupInfo")]
        public async Task<SubgroupInfo> GetSubgroupInfo(int subgroupId)
        {
            var subgroupInfo = await _groupService.GetSubgroupInfo(subgroupId);

            return subgroupInfo;
        }


        /// <summary>
        /// Добавление нового студента в подгруппу
        /// </summary>
        [HttpPost("Group/addNewStudent")]
        public async Task AddNewStudent([FromBody] AddNewStudentViewModel model)
        {
            await _groupService.AddNewStudentToSubgroup(model.SubgroupWithVersionId, model.Name, model.Comment);
        }

        /// <summary>
        /// Добавление существующего студента в подгруппу
        /// </summary>
        [HttpPost("Group/addExistingStudent")]
        public async Task AddExistingStudent([FromBody] AddExistingStudentViewModel model)
        {
            await _groupService.AddExistingStudentToSubgroup(model.SubgroupWithVersionId, model.StudentId);
        }

        /// <summary>
        /// Удаление студента из подгруппы
        /// </summary>
        [HttpPost("Group/deleteStudent")]
        public async Task DeleteStudent([FromBody] DeleteStudentViewModel model)
        {
            await _groupService.DeleteStudentFromSubgroup(model.SubgroupWithVersionId, model.StudentId);
        }

        /// <summary>
        /// Добавление студентов из файла Excel
        /// </summary>
        [HttpPost("Group/addStudentsFromExcel")]
        public async Task AddStudentsFromFile([FromForm] AddStudentFromExcelViewModel model)
        {
            await _groupService.AddStudentsToSubgroupFromFile(model.SubgroupWithVersionId, model.ExcelFile);
        }
        #endregion

        #region Управление связями
        /// <summary>
        /// Получение всех связей
        /// </summary>
        /// <returns>Список связей</returns>
        [HttpGet("Relation/getRelations")]
        public async Task<List<Relation>> GetRelations()
        {
            var relations = await _relationService.GetRelationsAsync();

            return relations;
        }


        /// <summary>
        /// Получение всех связей для страницы управления связями
        /// </summary>
        /// <returns>Список связей</returns>
        [HttpGet("Relation/getAdminRelationsInfo")]
        public async Task<List<AdminRelationInfo>> GetAdminRelationsInfo()
        {
            var relations = await _relationService.GetAdminRelationInfosAsync();

            return relations;
        }


        /// <summary>
        /// Получение всех пользователей с ролью "Преподаватель"
        /// </summary>
        /// <returns>Список преподавателей</returns>
        [HttpGet("Relation/getTeachers")]
        public async Task<List<User>> GetTeachers()
        {
            var teachers = await _userService.GetUsersWithRoleAsync("Teacher");

            return teachers;
        }

        /// <summary>
        /// Получение подгрупп последней версии группы
        /// </summary>
        [HttpGet("Relation/getSubgroups")]
        public async Task<List<string>> GetSubgroups(int mainGroupId)
        {
            var subgroups = await _groupService.GetSubgroupsNames(mainGroupId);

            return subgroups;

        }
        /// <summary>
        /// Добавление новой связи в систему
        /// </summary>
        [HttpPost("Relation/addRelation")]
        public async Task AddRelation([FromBody] RelationViewModel model)
        {
            await _relationService.AddRelationAsync(model);
        }

        /// <summary>
        /// Добавление связей из файла Excel 
        /// </summary>
        [HttpPost("Relation/addRelationFromExcel")]
        public async Task AddRelationFromExcel([FromForm] ExcelFileViewModel model)
        {
            await _relationService.AddRelationsFromFileAsync(model.ExcelFile);
        }
        #endregion

        #region Просмотр логов
        [HttpGet("getAllLogMessages")]
        public async Task<List<LogMessage>> GetAllLogMessages()
        {
            var messages = await _loggingService.GetAllLogMessages();

            return messages;
        }
        #endregion
    }
}
