namespace JournalAPI.Models.ViewModels
{
    public class MarkViewModel
    {
        public int MarkId { get; set; }
        public string? StudentName { get; set; }
        public int Module1 { get; set; }
        public int Module2 { get; set; }
        public int ExamOrTest { get; set; }
        public int? Coursework { get; set; }
        public Locked? Locked { get; set; }
    }
}
