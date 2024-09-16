namespace Five9.CodingAssessment.Models
{
    public class CallCenterEvent
    {
        public Guid AgentId { get; set; }
        public string AgentName { get; set; }
        public DateTime TimestampUtc { get; set; }
        public string Action { get; set; }
        public List<string> QueueIds { get; set; }
    }
}
