using Five9.CodingAssessment.Models;

namespace Five9.CodingAssessment.Data.Repositories
{
    public interface IAgentRepository
    {
        Task<Agent> GetAgentByIdAsync(Guid agentId);
        Task UpsertAgentAsync(Agent agent);
    }
}
