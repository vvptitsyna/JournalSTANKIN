using JournalAPI.Models.ServicesModels;

namespace JournalAPI.Services.Interfaces
{
    public interface IStudentService
    {
        public Task<List<StudentInfo>> GetAllStudents();
    }
}
