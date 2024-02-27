namespace JournalAPI.Models.StudentModels
{
    public class StudentSubgroup
    {
        public int Id { get; set; }
        public int SubgroupId { get; set; }
        public Subgroup Subgroup { get; set; }
        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
