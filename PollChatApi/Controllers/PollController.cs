using Microsoft.AspNetCore.Mvc;
using PollChatApi.Service;

namespace PollChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly PollService _pollMaker;

        public PollController(PollService pollMaker)
        {
            _pollMaker = pollMaker;
        }

        [HttpPost("create-weekly")]
        public async Task<IActionResult> CreateWeeklyPoll()
        {
            try
            {
                await _pollMaker.CreateNewPollAsync();
                return Ok("Poll created or already exists.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error: " +  ex.Message);
            }
        }

        [HttpPost("SettWinner/{pollId}")]
        public async Task<IActionResult> CheckWinner(int pollId)
        {
            try
            {
                await _pollMaker.SettWinner(pollId);
                return Ok("Winner Sett");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Server error" + ex.Message);
            }
        }
        [HttpPost("results/{pollId}")]
        public async Task<IActionResult> CountVotes(int pollId)
        {
            try
            {
                var results = await _pollMaker.CountVotes(pollId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error" + ex.Message);
            }
        }
    }
}
