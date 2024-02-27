namespace JournalAPI.Models.MarkModels
{
    [Flags]
    public enum Locked
    {
        None = 0,
        Module1 = 1,
        Module2 = 2,
        ExamOrTest = 4,
        Coursework = 8
    }

    public class Mark:BaseModel
    {
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int RelationId { get; set; }
        public Relation Relation { get; set; }
        public int Module1 { get; set; }
        public int Module2 { get; set; }
        public int ExamOrTest { get; set; }
        public int? Coursework { get; set; }

        /// <summary>
        /// Параметр, указывающий что запрещено редактировать
        /// </summary>
        public Locked Locked { get; set; }
    }
}
