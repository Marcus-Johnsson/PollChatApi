using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollChatApi.DAL;
using PollChatApi.DTO;
using PollChatApi.Model;

namespace PollChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThreadController : Controller
    {
        private readonly MyDbContext _db;
        public ThreadController(MyDbContext db)
        {  _db = db; }

        [HttpGet]
        public async Task<ActionResult<UserFavorites>> GetFavoriteSubject(string id)
        {
            try
            {
                var result = await ThreadManager.GetFavs(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error1: " + ex.Message);
            }
        }
        [HttpGet]
        public async Task<ActionResult<MainThread>> GetNewestThreads()
        {
            try
            {
                var result = await ThreadManager.GetNewestThreads();
                if (result == null)
                { return NotFound(); }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }
        [HttpGet("today")]
        public async Task<ActionResult<MainThread>> GetMostCommentsToday ()
        {
            try
            {
                var result = await ThreadManager.GetMostCommentsToday();
                if (result == null)
                { return NotFound(); }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }
        [HttpGet("week")]
        public async Task<ActionResult<MainThread>> GetMostCommentsWeek()
        {
            try
            {
                var result = await ThreadManager.GetMostCommentsWeek();
                if (result == null)
                { return NotFound(); }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }

        [HttpGet("filter/{userId}")]
        public async Task<IActionResult> GetFilteredSearch(
            string userId,
            [FromQuery] int subjectId,
            [FromQuery] int limit = 10,
            [FromQuery] string? afterCursor = null,
            [FromQuery] List<int>? subcategoryIds = null,
            [FromQuery] bool favoritesOnly = false)
        {
            try
            {
                var result = await ThreadManager.GetFilterdThreads(userId, subjectId, limit, afterCursor, subcategoryIds, favoritesOnly);
                if(result == null)
                { return NotFound(); }
                return Ok(result);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostThread([FromBody] MainThread thread)
        {
            try
            {
                var newThread = new MainThread
                {
                    UserId = thread.UserId,
                    SubjectId = thread.SubjectId,
                    SubCategory = thread.SubCategory,
                    Title = thread.Title,
                    Content = thread.Content,
                    CreatedAt = DateTime.UtcNow
                };
                await _db.SaveChangesAsync();

                return Ok(newThread);
                
            }
            catch (Exception ex)
            {
                return StatusCode(500,"server error: " +  ex.Message);            
            }

        }

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteThread([FromQuery] string userId, int id)
        {
            var result = await _db.MainThreads
                .Include(p=>p.Comments)
                .SingleOrDefaultAsync(p=>p.Id == id);
                

            if(result == null)
                { return NotFound(); }

            if(result.UserId != userId)
            {  return Forbid(); }

            foreach(var comment in  result.Comments)
            {
                comment.RemovedAt = DateTime.UtcNow;
            }

            result.RemovedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok();
        }



        //[HttpPost("createthread")]
        //public IActionResult PostCreateThread()
        //{

        //}

        //[HttpDelete("deletethread")]
        //public IActionResult DeleteThread()
        //{

        //}
    }
}