namespace JournalAPI.Models.GroupModels
{
    public class MainGroup:BaseModel
    {
        /// <summary>
        /// Название группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список версий группы
        /// </summary>
        public List<GroupWithVersion> GroupsWithVersion{ get; set; }
    }
}
