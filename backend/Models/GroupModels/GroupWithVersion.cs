namespace JournalAPI.Models.GroupModels
{
    public class GroupWithVersion:BaseModel,ICloneable
    {
        /// <summary>
        /// Название группы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Версия группы
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// Дата создания группы
        /// </summary>
        public string DateOfCreateon { get; set; }

        /// <summary>
        /// Id главной группы
        /// </summary>
        public int GroupId { get; set; }

        /// <summary>
        /// Главная группа
        /// </summary>
        public MainGroup Group { get; set; }

        /// <summary>
        /// Список подгрупп
        /// </summary>
        public List<Subgroup> Subgroups { get; set; }

        public object Clone()
        {
            // Создаем новый экземпляр класса GroupWithVersion
            var newGroupWithVersion = new GroupWithVersion
            {
                Name = this.Name,
                Version = this.Version + 1, // увеличиваем версию на 1
                GroupId = this.GroupId,
                Group = this.Group,
                Subgroups = new List<Subgroup>(),
                DateOfCreateon = DateTime.Now.ToString("G")
            };

            // Клонируем все подгруппы текущей группы
            foreach (var subgroup in this.Subgroups)
            {
                var newSubgroup = new Subgroup
                {
                    Name = subgroup.Name,
                    GroupWithVersion = newGroupWithVersion,
                    StudentSubgroups = new List<StudentSubgroup>()
                };
                
                // Клонируем всех студентов текущей подгруппы
                foreach (var studentSubgroup in subgroup.StudentSubgroups)
                {
                    var newStudentSubgroup = new StudentSubgroup
                    {
                        StudentId = studentSubgroup.StudentId,
                        Subgroup = newSubgroup
                    };

                    newSubgroup.StudentSubgroups.Add(newStudentSubgroup);
                }

                newGroupWithVersion.Subgroups.Add(newSubgroup);
            }

            return newGroupWithVersion;
        }

        public override bool Equals(object? obj)
        {
            if (obj is GroupWithVersion group)
                return group.Version == Version;

            return false;
        }

        public override int GetHashCode()
            => Version.GetHashCode();
    }
}
