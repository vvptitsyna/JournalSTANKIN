using JournalAPI.Models.RelationModels;

namespace JournalAPI.Models.SubjectModels
{
    public class Subject : BaseModel
    {
        public string Name { get; set; }
        public List<Relation> Relations { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is Subject subject)
                return subject.Name == Name;

            return false;
        }

        public override int GetHashCode()
            => HashCode.Combine(Name, Comment);
    }
}
