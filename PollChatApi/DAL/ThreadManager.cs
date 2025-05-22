using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollChatApi.Data.Dto;
using PollChatApi.DTO;
using PollChatApi.Model;

namespace PollChatApi.DAL
{
    public static class ThreadManager
    {
        private static MyDbContext _db;

        public static void Init(MyDbContext db)
        {
            _db = db;
        }


        public static async Task<List<MainThread>> GetNewestThreads()
        {
            try
            {
                return await _db.MainThreads
                .OrderByDescending(t => t.Id)
                .Take(4)
                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("MainThread not found");
            }
        }

        public static async Task<List<CommentCount>> GetMostCommentsToday()
        {
            var today = DateTime.Today;


            var CommentsCountToday = await _db.MainThreads
                .Select(t => new CommentCount
                {
                    Thread = t,
                    CommentsToday = t.Comments.Count(c => c.Date.Date == today)
                })
                .OrderByDescending(x => x.CommentsToday)
                .Take(4)
                .ToListAsync();

            return CommentsCountToday;
        }

        public static async Task<List<CommentCount>> GetMostCommentsWeek()
        {
            var today = DateTime.Today;

            var weekStart = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));

            var CommentsCountWeek = await _db.MainThreads
            .Select(t => new CommentCount
            {
                Thread = t,
                CommentsToday = t.Comments.Count(c => c.Date.Date >= weekStart && c.Date.Date <= today)
            })
            .OrderByDescending(x => x.CommentsToday)
            .Take(4)
            .ToListAsync();

            return CommentsCountWeek;
        }

        public static async Task<UserFavorites?> GetFavs(string userId)
        {
            var user = await _db.Users
               .Include(u => u.FavoriteSubcategories)
               .Include(u => u.FavoriteSubjects)
               .Include(u => u.FavoriteThreads)
               .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            { return null; }

            return new UserFavorites
            {
                Id = user.Id,
                FavoriteSubcategories = user.FavoriteSubcategories?.ToList() ?? new(),
                FavoriteSubjects = user.FavoriteSubjects?.ToList() ?? new(),
                FavoriteThreads = user.FavoriteThreads?.ToList() ?? new()
            };
        }


        public static async Task<ThreadFilterResult> GetFilterdThreads(
                        string userId,
            [FromQuery] int? subjectId,
            [FromQuery] int limit = 10,
            [FromQuery] string? afterCursor = null,
            [FromQuery] List<int>? subcategoryIds = null,
            [FromQuery] bool favoritesOnly = false
                        )
        {
            var query = _db.MainThreads.AsQueryable();

            if (subjectId.HasValue)
            {
                query = query.Where(t => t.SubjectId == subjectId.Value);
            }

            if (subcategoryIds != null && subcategoryIds.Count > 0)
            {
                query = query.Where(t => t.SubCategoryId.HasValue && subcategoryIds.Contains(t.SubCategoryId.Value));
            }

            if (favoritesOnly)
            {
                var favoriteIds = await GetFavoriteThreadIds(userId);
                query = query.Where(t => favoriteIds.Contains(t.Id));
            }

            if (!string.IsNullOrEmpty(afterCursor))
            {
                var parts = afterCursor.Split('_');
                if (parts.Length == 2 &&
                    long.TryParse(parts[0], out var ticks) &&
                    Guid.TryParse(parts[1], out var lastId))
                {
                    var cursorTime = new DateTime(ticks);
                    query = query.Where(t =>
                    t.CreatedAt < cursorTime ||
                    (t.CreatedAt == cursorTime && t.Id.CompareTo(lastId) == 0));
                }
            }

            var threads = await query.OrderByDescending(t => t.CreatedAt).
                                ThenBy(t => t.Id).
                                Take(limit + 1).
                                ToListAsync();

            var hasMore = threads.Count > limit;
            if (hasMore)
            {
                threads.RemoveAt(threads.Count - 1);
            }

            var nextCursor = hasMore ? $"{threads.Last().CreatedAt.Ticks}_{threads.Last().Id}" : null;

            return new ThreadFilterResult
            {
                Threads = threads,
                NextCursor = nextCursor,
                HasMore = hasMore
            };
        }
        public static async Task<List<int>> GetFavoriteThreadIds(string userId)
        {
            return await _db.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.FavoriteThreads.Select(ft => ft.Id))
                .ToListAsync();
        }

        public static async Task<IActionResult> UpdateThread([FromBody] ThreadEditDto dto)
        {
            var thread = await _db.MainThreads.Where(p => p.Id == dto.ThreadId).FirstOrDefaultAsync();

            var history = new ThreadEditHistory
            {
                ThreadId = thread.Id,
                OldTitle = thread.Title,
                OldContent = thread.Content,
                EditedTime = DateTime.UtcNow,
                EditedByUserId = dto.EditedByUserId,

            };
            if (thread == null)
            {
                return new NotFoundObjectResult($"Thread with ID {dto.ThreadId} not found.");
            }

            thread.Title = dto.Title;
            thread.Content = dto.Content;

            thread.SubjectId = dto.SubjectId;
            thread.SubCategoryId = dto.SubCategoryId;



            _db.ThreadHistory.Add(history);

            _db.SaveChanges();

            return new OkObjectResult(thread);
        }

        public static async Task<IActionResult> UpdateComment([FromBody] CommentEditHistory dto)
        {
            var oldComment = await _db.Comments.Where(p=>p.Id == dto.Id).SingleOrDefaultAsync();

            if(oldComment == null)
            {
                return new NotFoundObjectResult($"Comment with ID {dto.Id} not found.");
            }

            var history = new CommentEditHistory
            {
                Text = oldComment.Text,
                EditDate = DateTime.UtcNow,
                EditByUserId = dto.EditByUserId,
            };

            oldComment.Text = dto.Text;
            
            _db.CommentHistory.Add(history);

            _db.SaveChanges();

            return new OkObjectResult(oldComment);
        }

        public static async Task<IActionResult> GetThreadDetails(int id)
        {
            var thread = await _db.MainThreads
                .Include(p=>p.Comments)
                .ToListAsync();

            if (thread == null)
            { return new NotFoundObjectResult("Thread not found. Thread Id " + id); }

            return new OkObjectResult(thread);
        }



    }
}
