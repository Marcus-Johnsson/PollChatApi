using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollChatApi.DAL;
using PollChatApi.DTO;
using PollChatApi.Model;

namespace PollChatApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly AdminManager _adminManager;
        private readonly MyDbContext _db;
        public AdminController(MyDbContext db, AdminManager adminManager)
        {
            _db = db;
            _adminManager = adminManager;
        }

        [HttpPost("report")]
        public async Task<IActionResult> PostThread([FromBody] CreateWarningDto dto)
        { 
            try
            {
                var newWarning = new Warning
                {
                    UserId = dto.ObjectOwnerId,
                    Describtion = dto.Report,
                    CreatedAt = DateTime.UtcNow,
                    Type = dto.Type,
                    IsHandled = false,
                    ObjectsId = dto.ObjectId,
                    Scrap = false,
                    RepoUser = dto.RepoUser,
                };
                await _db.AddAsync(newWarning);
                await _db.SaveChangesAsync();

                return Ok(newWarning);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }

        }

        [HttpGet("threadwarnings")]
        public async Task<IActionResult> GetThreadWarningsList()
        {
            try
            {
                var result = await _adminManager.GetThreadWarnings();

                if (result == null)
                { return NotFound(); }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }

        }
        [HttpGet("commentwarnings")]
        public async Task<IActionResult> GetCommentWarningsList()
        {
            try
            {
                var result = await _adminManager.GetCommentWarnings();

                if (result == null)
                { return NotFound(); }

                return Ok(result);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }

        }

        [HttpPut("toggleobjects")]
        public async Task<IActionResult> UpdateCommentRemoveAdmin([FromBody] ReportInputDto dto)
        {
            
            if (dto == null)
                return NotFound();
            try
            {
                await _adminManager.PutThreadToggle(dto);


                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }
       




    }
}
