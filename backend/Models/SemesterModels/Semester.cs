using JournalAPI.Models.RelationModels;

namespace JournalAPI.Models.SemesterModels
{
    public enum Season
    {
        None,
        Fall,
        Spring
    }

    public class Semester:BaseModel
    {
        public int Year { get; set; }
        public Season Season { get; set; }
        public List<Relation> Relations { get; set; }
    }
}
