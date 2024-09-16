namespace Five9.CodingAssessment.Models
{
    public class Agent
    {
        public Guid AgentId { get; set; }
        public string AgentName { get; set; }
        public string State { get; set; }
        public List<Queue> Queues { get; set; }
    }
}
