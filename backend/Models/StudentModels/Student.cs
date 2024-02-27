namespace JournalAPI.Models.StudentModels
{
    public class Student:BaseModel
    {
        /// <summary>
        /// ФИО студента
        /// </summary>
        public string Name { get; set; }
        public List<StudentSubgroup> StudentSubgroups { get; set; }
        public List<Mark> Marks { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Student student)
                return Name == student.Name;

            return false;
        }

        public override int GetHashCode()
            => Name.GetHashCode();
    }
}
