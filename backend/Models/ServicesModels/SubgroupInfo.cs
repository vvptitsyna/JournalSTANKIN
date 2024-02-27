namespace JournalAPI.Models.ServicesModels
{
    public class SubgroupInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MainGroupName { get; set; }
        public string Comment { get; set; }
        public string GroupWithVersionName { get; set; }
        public int Version { get; set; }
        public List<StudentInfo> Students { get; set; }
    }
}
