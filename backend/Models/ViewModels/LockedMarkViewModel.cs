namespace JournalAPI.Models.ViewModels
{
    public class LockedMarkViewModel
    {
        public int MarkId { get; set; }
        public bool Module1Locked { get; set; }
        public bool Module2Locked { get; set; }
        public bool ExamOrTestLocked { get; set; }
        public bool CourseworkLocked { get; set; }
    }
}
