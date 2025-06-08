using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollChatApi.DAL;
using PollChatApi.DTO;
using PollChatApi.Model;
using PollChatApi.Service;

namespace PollChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ThreadController : Controller
    {
        private readonly ThreadManager _threadManager;
        private readonly MyDbContext _db;
        public ThreadController(MyDbContext db, ThreadManager threadManager)
        {
            _db = db;
            _threadManager = threadManager;
        }
        [HttpGet("threadlist")]
        public async Task<IActionResult> GetThreadList()
        {
            try
            {
               var result = await _threadManager.GetThreadList();

               if(result== null)
                { return NotFound(); }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }

        }
        [HttpGet("Getfavo")]
        public async Task<ActionResult<UserFavoritesDto>> GetFavoriteSubject(string id)
        {
            try
            {
                var result = await _threadManager.GetFavs(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error1: " + ex.Message);
            }
        }
        [HttpGet("newthreads")]
        public async Task<ActionResult<MainThread>> GetNewestThreads()
        {
            try
            {
                var result = await _threadManager.GetNewestThreads();
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
        public async Task<ActionResult<MainThreadDto>> GetMostCommentsToday()
        {
            try
            {
                var result = await _threadManager.GetMostCommentsToday();
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
        public async Task<ActionResult<MainThreadDto>> GetMostCommentsWeek()
        {
            try
            {
                var result = await _threadManager.GetMostCommentsWeek();
                if (result == null)
                { return NotFound(); }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }

        //[HttpGet("filter/{userId}")]
        //public async Task<IActionResult> GetFilteredSearch(
        //    string userId,
        //    [FromQuery] int subjectId,
        //    [FromQuery] int limit = 10,
        //    [FromQuery] string? afterCursor = null,
        //    [FromQuery] List<int>? subcategoryIds = null,
        //    [FromQuery] bool favoritesOnly = false)
        //{
        //    try
        //    {
        //        var result = await _threadManager.GetFilterdThreads(userId, subjectId, limit, afterCursor, favoritesOnly);
        //        if (result == null)
        //        { return NotFound(); }
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, "server error: " + ex.Message);
        //    }
        //}

        [HttpPost("createthread")]
        public async Task<IActionResult> PostThread([FromBody] ThreadWithImageDto dto)
        {
            try
            {

                
                var newThread = new MainThread
                {
                    UserId = dto.UserId,
                    SubjectId = dto.SubjectId,
                    ImagePath = dto.Image,
                    Title = dto.Title,
                    Content = dto.Text,
                    CreatedAt = DateTime.UtcNow
                };
                await _db.AddAsync(newThread);
                await _db.SaveChangesAsync();

                return Ok(newThread);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }

        }
        [HttpPost("createcomment")]
        public async Task<IActionResult> PostComment([FromBody] PostCommentDto dto)
        {
            try
            {
                var comment = new Comment
                {
                    UserId = dto.UserId,
                    Text = dto.Comment,
                    Date = DateTime.UtcNow,
                    ParentCommentId = dto.ParentCommentId,
                    ThreadId = dto.ThreadId
                };

                if (dto.ParentCommentId.HasValue)
                {
                    var parentComment = await _db.Comments.AnyAsync(p => p.Id == dto.ParentCommentId);
                    if (!parentComment)
                    {
                        return BadRequest("Parent comment not found");
                    }
                }

                await _db.AddAsync(comment);
                await _db.SaveChangesAsync();

                return Ok(comment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }

        [HttpPut("deletethread/{id}")]
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
        [HttpPut("deletecomment/{id}")]
        public async Task<IActionResult> DeleteComment([FromQuery] string userId, int id)
        {
            var comment = await _db.Comments
            .Include(c => c.Replies)
            .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            { return NotFound(); }

            if (!comment.ParentCommentId.HasValue)
            {
                foreach (var replies in comment.Replies)
                {
                    replies.RemovedAt = DateTime.UtcNow;
                }
            }
            else
            {
                comment.RemovedAt = DateTime.UtcNow;
            }

            await _db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("details/{id}")]
        public async Task<ActionResult<ThreadViewModel>> ThreadDetails(int id)
        {
            var thread = await _db.MainThreads
                .Include(t => t.Subject)
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (thread == null)
                return  NotFound();

            // Create the Comment tree
            var commentTree = _threadManager.BuildCommentTree(thread.Comments.ToList());

           
            var threadDto = new MainThreadDto
            {
                Id = thread.Id,
                Title = thread.Title,
                Content = thread.Content,
                ImagePath = thread.ImagePath,
                SubjectId = thread.SubjectId,
                UserId = thread.UserId,
                Subject = new SubjectDto
                {
                    Id = thread.Subject.Id,
                    Name = thread.Subject.Title
                },
                CreatedAt = thread.CreatedAt,
            };

            var viewModel = new ThreadViewModel
            {
                Thread = threadDto,
                CommentsTree = commentTree
            };

            return viewModel;
        }


        // api's to create....

        // UpdateThread check, GetThreadDetails check, AddComment check, UpdateComment check, DeleteComment, AddFavorite,VoteOnPoll


    }
}
