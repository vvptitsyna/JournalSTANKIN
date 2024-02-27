using JournalAPI.Models.RelationModels;
using JournalAPI.Models.SemesterModels;
using JournalAPI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JournalAPI.Services.Implementations
{
    public class MarkService : IMarkService
    {
        private readonly AppDbContext _context;
        private readonly IRelationService _relationService;

        public MarkService(AppDbContext context, IRelationService relationService)
        {
            _context = context;
            _relationService = relationService;
        }

        public async Task ChangeLockedMarksAsync(List<LockedMarkViewModel> marks)
        {
            var dbMarks = await _context.Marks.Where(m => marks.Select(mark => mark.MarkId).Contains(m.Id)).ToListAsync();

            marks.ForEach(mark =>
            {
                var dbMark = dbMarks.FirstOrDefault(m => m.Id == mark.MarkId);

                if (dbMark is null)
                    throw new Exception($"Оценка с Id {mark.MarkId} не найдена");

                if (mark.Module1Locked)
                    dbMark.Locked |= Locked.Module1;
                else
                    dbMark.Locked &= ~Locked.Module1;

                if (mark.Module2Locked)
                    dbMark.Locked |= Locked.Module2;
                else
                    dbMark.Locked &= ~Locked.Module2;

                if (mark.ExamOrTestLocked)
                    dbMark.Locked |= Locked.ExamOrTest;
                else
                    dbMark.Locked &= ~Locked.ExamOrTest;

                if (mark.CourseworkLocked)
                    dbMark.Locked |= Locked.Coursework;
                else
                    dbMark.Locked &= ~Locked.Coursework;
            });

            await _context.SaveChangesAsync();
        }
        public async Task<List<Mark>> GetMarksForReportAsync(int SemesterId, Locked locked)
        {
            if (!_context.Semesters.Any(s => s.Id == SemesterId))
                throw new Exception($"Не найден семестр с Id {SemesterId}");

            var marks = await _context.Marks
                    .Include(m => m.Relation)
                        .ThenInclude(r => r.Subgroup)
                            .ThenInclude(s => s.GroupWithVersion)
                    .Include(m => m.Student)
                    .ToListAsync();

            List<Mark> finmarks;

            if (locked == Locked.Coursework)
            {
                finmarks = marks.Where(m => m.Relation.SemesterId == SemesterId && !m.Locked.HasFlag(locked) && m.Relation.HasCoursework).ToList();
            }
            else
            {
                finmarks = marks.Where(m => m.Relation.SemesterId == SemesterId && !m.Locked.HasFlag(locked)).ToList();
            }

            return finmarks;
        }
        public async Task SaveMarksInReportAsync(int SemesterId, Locked locked)
        {
            if (!_context.Semesters.Any(s => s.Id == SemesterId))
                throw new Exception($"Не найден семестр с Id {SemesterId}");

            foreach (var mark in _context.Marks
                .Include(m => m.Relation)
                .Where(m => m.Relation.SemesterId == SemesterId))
            {
                mark.Locked |= locked;
            }

           await _context.SaveChangesAsync();
        }
        public async Task<FileContentResult> SaveReportInExcel(int SemesterId, Locked locked)
        {

            var marks = _context.Marks
                .Include(m => m.Relation)
                     .ThenInclude(r => r.Subject)
                .Include(m => m.Relation)
                    .ThenInclude(r => r.Subgroup)
                        .ThenInclude(s => s.GroupWithVersion)
                .Include(m => m.Student)
                .Where(m => m.Relation.SemesterId == SemesterId && !m.Locked.HasFlag(locked));

            var semester = await _context.Semesters.FirstOrDefaultAsync(s => s.Id == SemesterId);

            if(semester is null)
                throw new Exception($"Не найден семестр с Id {SemesterId}");

            if (locked == Locked.Coursework)
            {
                marks = marks.Where(m => m.Relation.HasCoursework);
            }

            string season = semester.Season == Season.Spring ? "Весна" : "Осень";

            var lockedName = locked switch
            {
                Locked.Module1 => "Модуль 1",
                Locked.Module2 => "Модуль 2",
                Locked.ExamOrTest => "Экзамен или зачет",
                Locked.Coursework => "Курсовая работа",
                _ => "Не выбрана форма контроля"
            };

            string filename = $"{semester.Year}г. {season} {lockedName}";

            byte[] data = ExcelHelper.WriteReportInExcel(filename, marks, lockedName, locked);

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            FileContentResult result = new FileContentResult(data, contentType);
            result.FileDownloadName = $"{filename}.xlsx";
            return result;
        }
        public async Task<FileContentResult> SaveMarksInExcel(int relationId)
        {
            Relation relation = await _relationService.GetRelationByIdAsync(relationId);

            if (relation is null)
                throw new Exception($"Не найдена связь с Id {relationId}");

            string season = relation.Semester.Season == Season.Spring ? "Весна" : "Осень";

            string filename = $"{relation.Subject.Name}  " +
                $"{relation.Subgroup.GroupWithVersion.Name}  ({relation.Subgroup.Name})" +
                $"Год: {relation.Semester.Year} {season}";

            byte[] data = ExcelHelper.WriteMarksInExcel(filename, relation);

            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            FileContentResult result = new FileContentResult(data, contentType);
            result.FileDownloadName = $"{filename}.xlsx";
            return result;
        }

        public async Task UpdateMarksAsync(List<MarkViewModel> marks)
        {
            var dbMarks = await _context.Marks
                .Include(m => m.Relation)
                .Where(m => marks.Select(mark => mark.MarkId)
                .Contains(m.Id)).ToListAsync();

            marks.ForEach(mark =>
            {
                var dbMark = dbMarks.FirstOrDefault(m => m.Id == mark.MarkId);

                if (dbMark is null)
                    throw new Exception($"Оценка с Id {mark.MarkId} не найдена");

                if (!dbMark.Locked.HasFlag(Locked.Module1))
                    dbMark.Module1 = mark.Module1;

                if (!dbMark.Locked.HasFlag(Locked.Module2))
                    dbMark.Module2 = mark.Module2;

                if (!dbMark.Locked.HasFlag(Locked.ExamOrTest))
                    dbMark.ExamOrTest = mark.ExamOrTest;

                if (!dbMark.Locked.HasFlag(Locked.Coursework) && dbMark.Relation.HasCoursework)
                    dbMark.Coursework = mark.Coursework;

            });

           await _context.SaveChangesAsync();
        }

    }
}
