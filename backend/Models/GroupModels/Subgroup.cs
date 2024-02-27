using JournalAPI.Models.RelationModels;

namespace JournalAPI.Models.GroupModels
{
    public class Subgroup:BaseModel
    {
        public string Name { get; set; }
        public int GroupWithVersionId { get; set; }
        public GroupWithVersion GroupWithVersion { get; set; }
        public List<Relation> Relations { get; set; }
        public List<StudentSubgroup> StudentSubgroups { get; set; }
    }
}
