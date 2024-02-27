using JournalAPI.Models.ServicesModels;
using Microsoft.EntityFrameworkCore;

namespace JournalAPI.Services.Implementations
{
    public class StudentService:IStudentService
    {
        private readonly AppDbContext _context;

        public StudentService(AppDbContext context)
            => _context = context;

        public async Task<List<StudentInfo>> GetAllStudents()
        {
            var students = await _context.Students
            .Include(s => s.StudentSubgroups)
                .ThenInclude(s => s.Subgroup)
                    .ThenInclude(s => s.GroupWithVersion)
                .ToListAsync();

            List<StudentInfo> studentInfos = new List<StudentInfo>();

            foreach (var student in students)
            {
                var lastSubgroup = student.StudentSubgroups.Select(s => s.Subgroup).OrderByDescending(s => s.GroupWithVersion.DateOfCreateon).First();

                studentInfos.Add(new StudentInfo
                {
                    Id = student.Id,
                    Name = student.Name,
                    Comment = student.Comment,
                    LastSubgroup = $"{lastSubgroup.GroupWithVersion.Name} ({lastSubgroup.Name})"
                });
            }

            return studentInfos;
        }
            
    }
}
