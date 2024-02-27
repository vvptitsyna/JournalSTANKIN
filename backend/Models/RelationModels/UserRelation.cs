namespace JournalAPI.Models.RelationModels
{
    public class UserRelation
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public User Teacher { get; set; }
        public int RelationId { get; set; }
        public Relation Relation { get; set; }
    }
}
