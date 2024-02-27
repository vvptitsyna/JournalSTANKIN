using JournalAPI.Models.ServicesModels;

namespace JournalAPI.Services.Interfaces
{
    public interface IGroupService
    {
        public Task<List<MainGroup>> GetGroupsAsync();
        public Task<List<string>> GetSubgroupsNames(int groupId);
        public Task<MainGroup> GetGroupAsync(int groupId);
        public Task AddGroupAsync(string name, string comment);
        public Task AddGroupsFromExcel(IFormFile formFile);
        public Task<List<GroupWithVersion>> GetGroupVersions(int groupId);
        public Task<List<GroupWithVersionInfo>> GetGroupVersionsInfo(int groupId);
        public Task<GroupWithVersionInfo> GetGroupWithVersionInfo(int groupWithVersionId);
        public Task<SubgroupInfo> GetSubgroupInfo(int subgroupId);
        public Task<GroupWithVersion> GetGroupWithVersion(int groupWithVersionId);
        public Task<Subgroup> GetSubgroup(int subgroupId);
        public Task EditGroupWithVersion(int groupWithVersionId, string name, string comment);
        public Task AddSubgroupToGroupWithVersion(int groupWithVersionId, string subgroupName, string comment);
        public Task DeleteSubgroupFromGroupWithVersion(int subgroupId);
        public Task AddNewStudentToSubgroup(int subgroupWithVersionId, string name, string comment);
        public Task AddExistingStudentToSubgroup(int subgroupWithVersionId, int studentId);
        public Task DeleteStudentFromSubgroup(int subgroupWithVersionId, int studentId);
        public Task CreateNewVersionOfGroup(int groupId);
        public Task AddStudentsToSubgroupFromFile(int subgroupWithVersionId, IFormFile formFile);
    }
}
