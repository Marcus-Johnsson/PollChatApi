using Microsoft.AspNetCore.Mvc;
using PollChatApi.Service;


namespace PollChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {
        private readonly IPollHandler _IpollHandler;



        public PollController(IPollHandler IpollHandler)
        {
            _IpollHandler = IpollHandler;
        }
           

       

        [HttpPost("SettWinner/{pollId}")]
        public async Task<IActionResult> CheckWinner(int pollId)
        {
            try
            {
                await _IpollHandler.SetWinner(pollId);
                return Ok("Winner Sett");
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Server error" + ex.Message);
            }
        }
        [HttpPost("results")]
        public async Task<IActionResult> CountVotes()
        {
            try
            {
                var results = await _IpollHandler.CountCurrentPoll();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error" + ex.Message);
            }
        }
    }
}
