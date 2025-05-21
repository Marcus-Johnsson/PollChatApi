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
        { _db = db; }

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
        public async Task<ActionResult<MainThread>> GetMostCommentsToday()
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
                if (result == null)
                { return NotFound(); }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }

        [HttpPost("create")]
        public async Task<IActionResult> PostThread([FromForm] ThreadWithImageDto dto)
        {
            try
            {
                string? imagePath = null;

                if (dto.Image != null)
                {
                    var fileName = Guid.NewGuid() + Path.GetExtension(dto.Image.FileName);
                    var filePath = Path.Combine("wwwroot", "userImage", fileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await dto.Image.CopyToAsync(stream);

                    imagePath = "/userImage" + fileName;

                }


                var newThread = new MainThread
                {
                    UserId = dto.UserId,
                    SubjectId = dto.SubjectId,
                    ImagePath = imagePath,
                    SubCategoryId = dto.SubCategoryId,
                    Title = dto.Title,
                    Content = dto.Content,
                    CreatedAt = DateTime.UtcNow
                };
                await _db.SaveChangesAsync();

                return Ok(newThread);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }

        }

        [HttpPut("delete/{id}")]
        public async Task<IActionResult> DeleteThread([FromQuery] string userId, int id)
        {
            var result = await _db.MainThreads
                .Include(p => p.Comments)
                .SingleOrDefaultAsync(p => p.Id == id);


            if (result == null)
            { return NotFound(); }

            if (result.UserId != userId)
            { return Forbid(); }

            foreach (var comment in result.Comments)
            {
                comment.RemovedAt = DateTime.UtcNow;
            }

            result.RemovedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();

            return Ok();
        }

        // api's to create....

       // Update Thread, GetThreadDetails, AddComment, UpdateComment, DeleteComment, AddFavorite,VoteOnPoll
    }
}