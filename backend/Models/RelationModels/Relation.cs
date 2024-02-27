using System.ComponentModel.DataAnnotations.Schema;

namespace JournalAPI.Models.RelationModels
{
    /// <summary>
    /// Что предусмотрено по этой связи экзамен или зачет
    /// </summary>
    public enum SubjectForm
    {
        Exam,
        Test
    }

    /// <summary>
    /// Класс для связи пользователя с семестром, предметом и группой. Также хранит оценки.
    /// </summary>
    public class Relation:BaseModel
    {
        /// <summary>
        /// Список преподавателей имеющих доступ к связи
        /// </summary>
        public List<UserRelation> Teachers { get; set; }

        /// <summary>
        /// Id Подгруппы
        /// </summary>
        public int SubgroupId { get; set; }

        /// <summary>
        /// Подгруппа
        /// </summary>
        public Subgroup Subgroup { get; set; }

        /// <summary>
        /// Id Предмета
        /// </summary>
        public int SubjectId { get; set; }

        /// <summary>
        /// Предмет 
        /// </summary>
        public Subject Subject { get; set; }

        /// <summary>
        /// Список оценок 
        /// </summary>
        public List<Mark> Marks { get; set; }

        /// <summary>
        /// Id Семестра
        /// </summary>
        public int SemesterId { get; set; }

        /// <summary>
        /// Семестр
        /// </summary>
        public Semester Semester { get; set; }

        /// <summary>
        /// ФИО лектора
        /// </summary>
        public string LecturerName { get; set; }

        /// <summary>
        /// Список ФИО преподавателей имеющих доступ к связи, разделение идет через;
        /// </summary>
        public string TeachersNames { get; set; }

        /// <summary>
        /// Параметр, указывающий что предусмотрено по этому предмету в семестре
        /// </summary>
        public SubjectForm SubjectForm { get; set; }

        /// <summary>
        /// Параметр, указывающий предусмотрена ли курсовая работа по этому предмету в семестре
        /// </summary>
        public bool HasCoursework { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Relation relation)
                return relation.LecturerName == LecturerName &&
                       relation.Semester == Semester &&
                       relation.Subject == Subject &&
                       relation.Subgroup == Subgroup;

            return false;
        }

        public override int GetHashCode()
            => HashCode.Combine(LecturerName, Semester,Subject,Subgroup);
    }
}
