using PollChatApi.Model;
using Microsoft.EntityFrameworkCore;
using PollChatApi.DTO;

namespace PollChatApi.Service
{
    public class ThreadService
    {
        private readonly MyDbContext _db;

        public ThreadService(MyDbContext db)
        {
            _db = db;
        }
        //public List<Comment> BuildCommentTree(List<Comment> allComments)
        //{
        //    var lookup = allComments.ToDictionary(c => c.Id);
        //    var roots = new List<Comment>();

        //    foreach (var comment in allComments)
        //    {
        //        if (comment.ParentCommentId == null)
        //        {
        //            roots.Add(comment);
        //        }
        //        else if (lookup.TryGetValue(comment.ParentCommentId.Value, out var parent))
        //        {
        //            parent.Replies.Add(comment);
        //        }
        //    }

        //    return roots;
        //}

        //public async Task<List<MainThread>> GetNewestThreads()
        //{
        //    try
        //    {
        //        return await _db.MainThreads
        //        .OrderByDescending(t => t.Id)
        //        .Take(4)
        //        .ToListAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("MainThread not found");
        //    }
        //}

        //public async Task<List<CommentCountDto>> GetMostCommentsToday()
        //{
        //    var today = DateTime.Today;


        //    var CommentsCountToday = await _db.MainThreads
        //        .Select(t => new CommentCountDto
        //        {
        //            Thread = t,
        //            CommentsToday = t.Comments.Count(c => c.Date.Date == today)
        //        })
        //        .OrderByDescending(x => x.CommentsToday)
        //        .Take(4)
        //        .ToListAsync();

        //    return CommentsCountToday;
        //}

        //public async Task<List<CommentCountDto>> GetMostCommentsWeek()
        //{
        //    var today = DateTime.Today;

        //    var weekStart = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));

        //    var CommentsCountWeek = await _db.MainThreads
        //    .Select(t => new CommentCountDto
        //    {
        //        Thread = t,
        //        CommentsToday = t.Comments.Count(c => c.Date.Date >= weekStart && c.Date.Date <= today)
        //    })
        //    .OrderByDescending(x => x.CommentsToday)
        //    .Take(4)
        //    .ToListAsync();

        //    return CommentsCountWeek;
        //}

        //public async Task<User?> GetFavs(string userId)
        //{
        //    var result = await _db.Users
        //        .Include(f => f.FavoriteSubjects)
        //        .Include(f => f.FavoriteThreads)
        //        .FirstOrDefaultAsync(p => p.Id == userId);


        //    return result;
        //}
    }
}
