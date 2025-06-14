using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollChatApi.DTO;
using PollChatApi.Model;

namespace PollChatApi.DAL
{
    public class ThreadManager
    {
        private readonly MyDbContext _db;

        public ThreadManager(MyDbContext db)
        {
            _db = db;
        }
        public async Task<List<MainThreadDto>> GetThreadList()
        {
            var threads = await _db.MainThreads
                .Include(i=>i.Subject)
                .Select(p => new MainThreadDto
            {
                Content = p.Content,
                Title = p.Title,
                CommentCount = p.Comments.Count(),
                ImagePath = p.ImagePath,
                CreatedAt = p.CreatedAt,
                Id = p.Id,
                UserId = p.UserId,
                    Subject = p.Subject != null ? new SubjectDto
                    {
                        Id = p.SubjectId,
                        Name = p.Subject.Title
                    } : null
                })
                .ToListAsync();


            

            return threads; 
        }

        public async Task<List<NewestThreadDto>> GetNewestThreads()
        {
            try
            {
                 return await _db.MainThreads
                     .Where(p => p.RemovedAt == null)
                     .OrderByDescending(t => t.Id)
                     .Take(4)
                     .Select(p => new NewestThreadDto()
                     {
                         Id = p.Id,
                         Title = p.Title,
                         CommentCount = p.Comments.Count()
                     })
                     .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("MainThread not found");
            }
        }

        public async Task<List<MainThreadDto>> GetMostCommentsToday()
        {
            var today = DateTime.Today;

            var threads = await _db.MainThreads
                    .Where(t => t.RemovedAt == null)
                    .Select(t => new MainThreadDto
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Content = t.Content,
                        ImagePath = t.ImagePath,
                        SubjectId = t.SubjectId,
                        Subject = new SubjectDto
                        {
                            Id = t.Subject.Id,
                            Name = t.Subject.Title
                        },
                        CreatedAt = t.CreatedAt,
                        CommentCount = t.Comments.Count(c => c.Date.Date == today) // Thread can be old but comment count is same day
                    })
                            .OrderByDescending(x => x.CommentCount)
                .Take(4)
                .ToListAsync();

            return threads;
        }

        public async Task<List<MainThreadDto>> GetMostCommentsWeek()
        {
            var today = DateTime.Today;

            var weekStart = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));

            var threads = await _db.MainThreads
                     .Where(t => t.RemovedAt == null)
                     .Select(t => new MainThreadDto
                     {
                         Id = t.Id,
                         Title = t.Title,
                         Content = t.Content,
                         ImagePath = t.ImagePath,
                         SubjectId = t.SubjectId,
                         Subject = new SubjectDto
                         {
                             Id = t.Subject.Id,
                             Name = t.Subject.Title
                         },
                         CreatedAt = t.CreatedAt,

                         CommentCount = t.Comments.Count(c => c.Date.Date >= weekStart && c.Date.Date <= today) // Thread can be old but comment count is same day
                     })
            .OrderByDescending(x => x.CommentCount)
            .Take(4)
            .ToListAsync();

            return threads;
        }

        public async Task<UserFavoritesDto?> GetFavs(string userId)
        {
            var user = await _db.Users
               .Include(u => u.FavoriteSubjects)
               .Include(u => u.FavoriteThreads)
               .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            { return null; }

            return new UserFavoritesDto
            {

                Id = userId,
                FavoriteSubjects = user.FavoriteSubjects?.Select(s => new SubjectDto
                {
                    Id = s.Id,
                    Name = s.Title
                }).ToList() ?? new List<SubjectDto>(),

                FavoriteThreads = user.FavoriteThreads?.Select(x => new MainThreadDto
                {
                    Id = x.Id,
                    Title = x.Title,
                    Content = x.Content,
                    ImagePath = x.ImagePath,
                    SubjectId = x.SubjectId,
                    Subject = new SubjectDto
                    {
                        Id = x.Subject.Id,
                        Name = x.Subject.Title
                    },
                    CreatedAt = x.CreatedAt
                }).ToList() ?? new List<MainThreadDto>()
            };
        }


        //public async Task<ThreadFilterResult> GetFilterdThreads(
        //                string userId,
        //    [FromQuery] int? subjectId,
        //    [FromQuery] int limit = 10,
        //    [FromQuery] string? afterCursor = null,
        //    [FromQuery] bool favoritesOnly = false
        //                )
        //{
        //    var query = _db.MainThreads.AsQueryable();

        //    if (subjectId.HasValue)
        //    {
        //        query = query.Where(t => t.SubjectId == subjectId.Value);
        //    }

        //    if (favoritesOnly)
        //    {
        //        var favoriteIds = await GetFavoriteThreadIds(userId);
        //        query = query.Where(t => favoriteIds.Contains(t.Id));
        //    }

        //    if (!string.IsNullOrEmpty(afterCursor))
        //    {
        //        var parts = afterCursor.Split('_');
        //        if (parts.Length == 2 &&
        //            long.TryParse(parts[0], out var ticks) &&
        //            Guid.TryParse(parts[1], out var lastId))
        //        {
        //            var cursorTime = new DateTime(ticks);
        //            query = query.Where(t =>
        //            t.CreatedAt < cursorTime ||
        //            (t.CreatedAt == cursorTime && t.Id.CompareTo(lastId) == 0));
        //        }
        //    }

        //    var threads = await query.OrderByDescending(t => t.CreatedAt).
        //                        ThenBy(t => t.Id).
        //                        Take(limit + 1).
        //                        ToListAsync();

        //    var hasMore = threads.Count > limit;
        //    if (hasMore)
        //    {
        //        threads.RemoveAt(threads.Count - 1);
        //    }

        //    var nextCursor = hasMore ? $"{threads.Last().CreatedAt.Ticks}_{threads.Last().Id}" : null;

        //    return new ThreadFilterResult
        //    {
        //        Threads = threads,
        //        NextCursor = nextCursor,
        //        HasMore = hasMore
        //    };
        //}


        public async Task<IActionResult> UpdateThread([FromBody] ThreadEditDto dto)
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


            _db.ThreadHistory.Add(history);

            _db.SaveChanges();

            return new OkObjectResult(thread);
        }

        public async Task<IActionResult> UpdateComment([FromBody] CommentEditHistory dto)
        {
            var oldComment = await _db.Comments.Where(p => p.Id == dto.Id).SingleOrDefaultAsync();

            if (oldComment == null)
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

        public async Task<IActionResult> GetThreadDetails(int id) 
        {
            var thread = await _db.MainThreads
                .Where(p=> p.Id == id)
                .Include(p => p.Comments)
                
                .ToListAsync();

            if (thread == null)
            { return new NotFoundObjectResult("Thread not found. Thread Id " + id); }

            return new OkObjectResult(thread);
        }

        public List<CommentDto> BuildCommentTree(List<Comment> allComments)
        {
            var commentDtoLookup = allComments.ToDictionary(
                    c => c.Id,
                    c => new CommentDto
                    {
                        UserId = c.UserId,
                        Id = c.Id,
                        ParentCommentId = c.ParentCommentId,
                        Text = c.Text,
                        Date = c.Date,
                        Replies = new List<CommentDto>(),
                        RemovedByAdmin = c.RemovedByAdmin,
                        RemovedByUser = c.RemovedByUser,
                        DepthValue = 0,
                        ThreadId =c.ThreadId
                    });
            
            var branches = new List<CommentDto>();

            foreach (var comment in allComments)
            {
                if (comment.ParentCommentId == null)
                {
                    
                    branches.Add(commentDtoLookup[comment.Id]);
                }
                else if (commentDtoLookup.TryGetValue(comment.ParentCommentId.Value, out var parent))
                {
                    var currentComment = commentDtoLookup[comment.Id];
                    parent.Replies.Add(currentComment);
                    currentComment.DepthValue = parent.DepthValue + 1;
                }
            }

            var commentOrder = new List<CommentDto>();
            foreach (var comment in branches)
            {
                if(comment.RemovedByAdmin == true)
                {
                    comment.Text = "Comment removed by admin. Behave!";
                }
                if (comment.RemovedByUser == true)
                {
                    comment.Text = "Comment removed by user. Chicken! BWAK BAWK";
                }
                
            }

            FlatTheTree(branches);

            void FlatTheTree(List<CommentDto> branches)
            {
                foreach (var comment in branches)
                {
                        commentOrder.Add(comment);
                  
                    if (comment.Replies != null && comment.Replies.Any())
                    {
                        FlatTheTree(comment.Replies.ToList());
                    }
                }
            }

            return commentOrder;
        }


    }
}
