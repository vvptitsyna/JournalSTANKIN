namespace JournalAPI.Models.ViewModels
{
    public class RelationViewModel
    {
        public int LecturerId { get; set; }
        public List<int> TeachersId { get; set; }
        public int SubjectId { get; set; }
        public int GroupId { get; set; }
        public string SubgroupName { get; set; }
        public int SemesterId { get; set; }
        public SubjectForm SubjectForm { get; set; }
        public bool HasCoursework { get; set; }
    }
}
