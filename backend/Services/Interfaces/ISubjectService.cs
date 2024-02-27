namespace JournalAPI.Services.Interfaces
{
    public interface ISubjectService
    {
        public Task AddSubjectsFromFileAsync(IFormFile file);
        public Task AddSubjectAsync(string name, string comment);
        public Task<List<Subject>> GetSubjectsAsync();
    }
}
