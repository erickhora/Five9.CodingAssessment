using Five9.CodingAssessment.Models;
using Microsoft.EntityFrameworkCore;

namespace Five9.CodingAssessment.Data.Repositories
{
    public class PostgreSqlAgentRepository : IAgentRepository
    {
        private readonly CallCenterContext _context;

        public PostgreSqlAgentRepository(CallCenterContext context)
        {
            _context = context;
        }

        public async Task<Agent> GetAgentByIdAsync(Guid agentId)
        {
            return await _context.Agents.FirstOrDefaultAsync(a => a.AgentId == agentId);
        }

        public async Task UpsertAgentAsync(Agent agent)
        {
            _context.Agents.Update(agent);
            await _context.SaveChangesAsync();
        }
    }

}
