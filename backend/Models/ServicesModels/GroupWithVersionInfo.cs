namespace JournalAPI.Models.ServicesModels
{
    public class GroupWithVersionInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public string DateOfCreateon { get; set; }
        public string Comment { get; set; }
        public string MainGroupName { get; set; }
        public List<SubgroupInfo> Subgroups { get; set; }
    }
}
