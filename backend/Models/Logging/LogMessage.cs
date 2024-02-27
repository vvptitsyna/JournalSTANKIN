namespace JournalAPI.Models.Logging
{
    public enum UserAction
    {
        Add,
        AddFromExcel,
        Remove,
        Edit,
        SaveInExcel,
        SaveMarks,
        SaveReport,
    }

    public class LogMessage
    {
        public int Id {get; set;}
        public bool Success {get; set;}
        public string DateTime {get; set;}
        public UserAction? Action { get; set;}
        public string UserName {get; set;}
        public string Message {get; set;}
        public string? ExceptionMessage { get; set;}
    }
}
