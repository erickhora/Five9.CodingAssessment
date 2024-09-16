using Five9.CodingAssessment.Models;
using MongoDB.Driver;

namespace Five9.CodingAssessment.Data.Repositories
{
    public class MongoDbAgentRepository : IAgentRepository
    {
        private readonly IMongoCollection<Agent> _agentCollection;

        public MongoDbAgentRepository(IMongoDatabase database)
        {
            _agentCollection = database.GetCollection<Agent>("Agents");
        }

        public async Task<Agent> GetAgentByIdAsync(Guid agentId)
        {
            return await _agentCollection.Find(a => a.AgentId == agentId).FirstOrDefaultAsync();
        }

        public async Task UpsertAgentAsync(Agent agent)
        {
            var filter = Builders<Agent>.Filter.Eq(a => a.AgentId, agent.AgentId);
            await _agentCollection.ReplaceOneAsync(filter, agent, new ReplaceOptions { IsUpsert = true });
        }
    }

}
