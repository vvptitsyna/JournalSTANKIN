using JournalAPI.Models.ViewModels;

namespace JournalAPI.Models.ServicesModels
{
    public class RelationInfo
    {
        public string LecturerName { get; set; }
        public List<string> TeacherNames { get; set; }
        public string Comment { get; set; }
        public List<MarkViewModel> Marks { get; set; }

    }
}
