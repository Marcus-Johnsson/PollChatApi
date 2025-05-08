using Microsoft.AspNetCore.Mvc;
using PollChatApi.Service;


namespace PollChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly IPollService _pollService;

        public PollController(IPollService pollService)
        {
            _pollService = pollService;
        }

       

        [HttpPost("SettWinner/{pollId}")]
        public async Task<IActionResult> CheckWinner(int pollId)
        {
            try
            {
                await _pollService.SetWinner(pollId);
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
                var results = await _pollService.CountVotes(pollId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error" + ex.Message);
            }
        }
    }
}
