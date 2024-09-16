using Five9.CodingAssessment.Data;
using Five9.CodingAssessment.Models;
using Five9.CodingAssessment.Extensions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Five9.CodingAssessment.Data.Repositories;

namespace Five9.CodingAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallCenterController : ControllerBase
    {
        private readonly IAgentRepository _agentRepository;
        public CallCenterController(IAgentRepository agentRepository)
        {
            _agentRepository = agentRepository;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessEvent([FromBody] CallCenterEvent callEvent)
        {
            DateTime currentUtcTime = DateTime.UtcNow;

            if (currentUtcTime.Subtract(callEvent.TimestampUtc).TotalHours > 1)
            {
                return BadRequest(new LateEventException("Event is more than an hour old"));
            }

            var agent = await _agentRepository.GetAgentByIdAsync(callEvent.AgentId)
                        ?? new Agent { AgentId = callEvent.AgentId, AgentName = callEvent.AgentName };

            switch (callEvent.Action)
            {
                case "START_DO_NOT_DISTURB" when callEvent.TimestampUtc.Hour >= 11 && callEvent.TimestampUtc.Hour < 13:
                    agent.State = "ON_LUNCH";
                    break;
                case "CALL_STARTED":
                    agent.State = "ON_CALL";
                    break;
                default:
                    return BadRequest("Unknown action");
            }

            agent.Queues = callEvent.QueueIds.Select(queueId => new Queue { QueueId = Guid.Parse(queueId) }).ToList();

            await _agentRepository.UpsertAgentAsync(agent);

            return Ok(agent);
        }
    }
}
