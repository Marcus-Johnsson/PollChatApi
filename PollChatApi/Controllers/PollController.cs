using Microsoft.AspNetCore.Mvc;
using PollChatApi.Service;
using PollChatApi.DAL;


namespace PollChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PollController : ControllerBase
    {

           
        
        [HttpGet("results")]
        public async Task<IActionResult> CountVotes()
        {
            try
            {
                var results = await PollManager.CountCurrentPoll();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server error" + ex.Message);
            }
        }
    }
}
