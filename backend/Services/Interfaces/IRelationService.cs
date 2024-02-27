using JournalAPI.Models.ServicesModels;
using JournalAPI.Models.ViewModels;

namespace JournalAPI.Services.Interfaces
{
    public interface IRelationService
    {
        public Task<List<Relation>> GetRelationsAsync();
        public Task<List<AdminRelationInfo>> GetAdminRelationInfosAsync();
        public Task<List<Relation>> GetRelationsForSemesterAsync(int semesterId);
        public Task AddRelationAsync(RelationViewModel relation);
        public Task AddRelationsFromFileAsync(IFormFile file);
        public Task<List<SubjectInfo>> GetSubjectsInfo(string username, int semesterId);
        public Task<Relation> GetRelationByIdAsync(int relationId);
        public Task<RelationInfo> GetRelationInfoAsync(int relationId);
        public Task<List<GroupInfo>> GetGroupsInfo(string username, int semesterId, int subjectId);
    }
}
