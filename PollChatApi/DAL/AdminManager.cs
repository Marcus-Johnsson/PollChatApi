using Microsoft.EntityFrameworkCore;
using PollChatApi.DTO;
using PollChatApi.Model;

namespace PollChatApi.DAL
{
    public class AdminManager
    {
        private readonly MyDbContext _db;

        public AdminManager(MyDbContext db)
        {
            _db = db;
        }

        public async Task<List<WarningDto>> GetThreadWarnings()
        {
            var warnings = await _db.Warnings.Where(p => p.Type == "Thread")
                .Select(o => new WarningDto()
                {
                    AdminId = o.AdminId,
                    CreatedAt = o.CreatedAt,
                    ObjectsId = o.ObjectsId,
                    Describtion = o.Describtion,
                    HandeldAtTime = o.HandeldAtTime,
                    Type = o.Type,
                    Id = o.Id,
                    UserId = o.UserId,
                    Scrap = o.Scrap,
                    RepoUser = o.RepoUser,

                })
                .ToListAsync();


            return warnings;
        }

        public async Task<List<WarningDto>> GetCommentWarnings()
        {
            var warnings = await _db.Warnings.Where(p => p.Type == "Comment")
                .Select(o => new WarningDto()
                {
                    AdminId = o.AdminId,
                    CreatedAt = o.CreatedAt,
                    ObjectsId = o.ObjectsId,
                    Describtion = o.Describtion,
                    HandeldAtTime = o.HandeldAtTime,
                    Type = o.Type,
                    Id = o.Id,
                    UserId = o.UserId,
                    Scrap = o.Scrap

                })
                .ToListAsync();

            return warnings;
        }

        public async Task PutThreadToggle(ReportInputDto dto)
        {
            if (dto.ObjectType == "Thread")
            {
                var thread = _db.MainThreads.Where(p => p.Id == dto.ObjectId)
                    .IgnoreQueryFilters()
                    .FirstOrDefault();

                thread.RemovedByAdmin = dto.Toggle;
                thread.RemovedAt = dto.Toggle == false ? null : DateTime.UtcNow;

                _db.Update(thread);
            }
            else
            {
                var comment = _db.Comments.Where(p => p.Id == dto.ObjectId).FirstOrDefault();

                comment.RemovedByAdmin = dto.Toggle;
                comment.RemovedAt = dto.Toggle == false ? null : DateTime.UtcNow;

                _db.Update(comment);
            }

            var warning = await _db.Warnings.Where(p => p.Id == dto.WarningId).FirstOrDefaultAsync();

            if (dto.Action == "Scrap")
            {
                warning.Scrap = true;
                warning.HandeldAtTime = DateTime.UtcNow;
                warning.AdminId = dto.AdminId;
                warning.IsHandled = true;

                _db.Update(warning);
            }

            if (dto.Action == "Remove")
            {
                warning.Scrap = false;
                warning.HandeldAtTime = DateTime.UtcNow;
                warning.AdminId = dto.AdminId;
                warning.IsHandled = true;
            }

            if (dto.Action == "Reverse")
            {
                warning.Scrap = false;
                warning.HandeldAtTime = DateTime.UtcNow;
                warning.AdminId = dto.AdminId;
                warning.IsHandled = true;
            }



            _db.Update(warning);


            await _db.SaveChangesAsync();

        }
    }
}
