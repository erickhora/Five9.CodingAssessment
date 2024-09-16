using Five9.CodingAssessment.Data;
using Five9.CodingAssessment.Models;
using Five9.CodingAssessment.Extensions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Five9.CodingAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CallCenterController : ControllerBase
    {
        private readonly CallCenterContext _context;

        public CallCenterController(CallCenterContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessEvent([FromBody] CallCenterEvent callEvent)
        {
            DateTime currentUtcTime = DateTime.UtcNow;

            if (currentUtcTime.Subtract(callEvent.TimestampUtc).TotalHours > 1)
            {
                return BadRequest(new LateEventException("Event is more than 1 hour old"));
            }

            var agent = await _context.Agents.FirstOrDefaultAsync(a => a.AgentId == callEvent.AgentId) ?? new Agent { AgentId = callEvent.AgentId, AgentName = callEvent.AgentName};

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

            _context.Agents.Update(agent);
            await _context.SaveChangesAsync();

            return Ok(agent);
        }
    }
}
