namespace JournalAPI.Services.Interfaces
{
    public interface ISemesterService
    {
        public Task AddSemesterAsync(int year, Season season);
        public Task GenerateSemestersAsync(int yearStart, int yearEnd);
        public Task<List<Semester>> GetSemestersAsync();
        public Task<List<Semester>> GetSemestersForUserAsync(string username);
        public Task<Semester> GetSemesterByIdAsync(int semesterId);
        public Task<string> GetSemesterName(int semesterId);

    }
}
