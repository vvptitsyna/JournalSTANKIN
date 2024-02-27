using JournalAPI.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Sorting;
using System.Collections;

namespace JournalAPI.Services.Implementations
{
    public class SemesterService : ISemesterService
    {
        private readonly AppDbContext _context;

        public SemesterService(AppDbContext context)
            => _context = context;

        public async Task AddSemesterAsync(int year, Season season)
        {
            if (SemesterExist(year, season))
                throw new Exception("Такой семестр уже существует");

            if (year <= 2000)
                throw new Exception("Год не может быть меньше 0");

            await _context.Semesters.AddAsync(new Semester
            {
                Year = year,
                Season = season
            });

            await _context.SaveChangesAsync();
        }

        public async Task<List<Semester>> GetSemestersForUserAsync(string userName)
        {
            var user = await _context.Users.FirstAsync(u => u.UserName == userName);

            if (user is null)
                throw new Exception($"Пользователь с логином {userName} не найден");

            var semesters = await _context.Relations
                .Include(u => u.Semester)
                .Where(u => u.Teachers.Select(t => t.TeacherId).Contains(user.Id) || u.LecturerName == user.UserName)
                .Select(u => u.Semester).Distinct().ToListAsync();

            return semesters;
        }

        public async Task<List<Semester>> GetSemestersAsync()
            => await _context.Semesters.ToListAsync();

        public async Task<string> GetSemesterName(int semesterId)
        {
            var semester = await GetSemesterByIdAsync(semesterId);

            string name = $"{semester.Year} ";
            name += semester.Season == Season.Fall ? "Осень" : "Весна";
            return name;
        }

        private bool SemesterExist(int year, Season season)
            => _context.Semesters.Any(s => s.Year == year && s.Season == season);

        public async Task GenerateSemestersAsync(int yearStart, int yearEnd)
        {
            for (int year = yearStart; year <= yearEnd; year++)
            {
                await AddSemesterAsync(year, Season.Fall);
                await AddSemesterAsync(year, Season.Spring);
            }

            await _context.SaveChangesAsync();
        }
        public async Task<Semester> GetSemesterByIdAsync(int semesterId)
        {
            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Id == semesterId);

            if(semester is null)
                throw new Exception($"Семестр с Id {semesterId} не найден");

            return semester;
        }

    }
}
