using Microsoft.AspNetCore.Mvc;
using PollChatApi.Service;
using PollChatApi.DAL;


namespace PollChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly PollManager _pollManager;

        public PollController(PollManager pollManager)
        {
            _pollManager = pollManager;
        }

        [HttpGet("results")]
        public async Task<IActionResult> CountVotes()
        {
            try
            {
                var results = await _pollManager.CountCurrentPoll();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error1111111111111111111111111" + ex.Message);
            }
        }
    }
}
