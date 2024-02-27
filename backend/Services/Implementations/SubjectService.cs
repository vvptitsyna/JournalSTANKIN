using Microsoft.EntityFrameworkCore;

namespace JournalAPI.Services.Implementations
{
    public class SubjectService:ISubjectService
    {
        private readonly AppDbContext _context;

        public SubjectService(AppDbContext context)
            => _context = context;

        public async Task AddSubjectAsync(string name, string comment)
        {
            if (SubjectExist(name, comment))
                throw new Exception($"Предмет с названием {name} и комментарием {comment} уже существует");

            await _context.Subjects.AddAsync(new Subject { Name = name , Comment = comment});
            await _context.SaveChangesAsync();
        }

        public async Task AddSubjectsFromFileAsync(IFormFile file)
        {
            var subjects = ExcelHelper.GetSubjectsFromExcel(file);

            var subjectsNames = subjects.Intersect(_context.Subjects).Select(s => new { s.Name, s.Comment });

            if (subjectsNames.Any())
            {
                    string fault = "";
                    foreach (var subject in subjectsNames)
                    {
                        fault += $"Предмет с названием с названием \"{subject.Name}\" и комментарием:\"{subject.Comment}\" уже существует {Environment.NewLine}";
                    }
                    throw new Exception(fault);
            }

            await _context.Subjects.AddRangeAsync(subjects);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Subject>> GetSubjectsAsync()
            => await _context.Subjects.ToListAsync();

        private bool SubjectExist(string name, string comment)
            => _context.Subjects.Any(s => s.Name == name && s.Comment == comment);
    }
}
