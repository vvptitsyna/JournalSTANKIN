namespace JournalAPI.Models
{
    /// <summary>
    /// Базовый класс, куда вынесены поля используемые в остальных классах
    /// </summary>
    public abstract class BaseModel
    {
        public int Id { get; set; }
        public string? Comment { get; set; }
    }
}
