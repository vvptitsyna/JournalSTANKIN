namespace JournalAPI.Models.ViewModels.Group
{
    public class AddStudentFromExcelViewModel
    {
        public IFormFile ExcelFile { get; set; }
        public int SubgroupWithVersionId { get; set; }
    }
}
