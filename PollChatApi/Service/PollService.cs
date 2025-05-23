using Microsoft.EntityFrameworkCore;
using PollChatApi.DTO;
using PollChatApi.Model;

namespace PollChatApi.Service
{
    public class PollServices
    {
        private readonly MyDbContext _db;

        public PollServices(MyDbContext db)
        {
            _db = db;
        }





       

        public async Task<List<PollResultDto>> CountCurrentPoll()
        {
            var latestPoll = await _db.Polls
                .Include(p => p.Votes)
                .FirstOrDefaultAsync();

            if (latestPoll == null)
            {
                throw new Exception("Poll not found");
            }

            var results = latestPoll.Votes
                .GroupBy(v => v.SubjectId)
                .Join(_db.Subjects,
                      voteGroup => voteGroup.Key,
                      subject => subject.Id,
                      (voteGroup, subject) => new PollResultDto
                      {
                          SubjectId = subject.Id,
                          Title = subject.Title,
                          Votes = voteGroup.Count()
                      })
                .ToList();

            return results;
        }

        public async Task<List<MainThread>> NewestThreads()
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

        public async Task<List<CommentCountDto>> MostCommentsToday()
        {
            var today = DateTime.Today;


            var CommentsCountToday = await _db.MainThreads
                .Select(t => new CommentCountDto
                {
                    Thread = t,
                    CommentsToday = t.Comments.Count(c => c.Date.Date == today)
                })
                .OrderByDescending(x => x.CommentsToday)
                .Take(4)
                .ToListAsync();

            return CommentsCountToday;
        }

        public async Task<List<CommentCountDto>> MostCommentsWeek()
        {
            var today = DateTime.Today;

            var weekStart = today.AddDays(-(int)today.DayOfWeek + (today.DayOfWeek == DayOfWeek.Sunday ? -6 : 1));

            var CommentsCountWeek = await _db.MainThreads
            .Select(t => new CommentCountDto
            {
                Thread = t,
                CommentsToday = t.Comments.Count(c => c.Date.Date >= weekStart && c.Date.Date <= today)
            })
            .OrderByDescending(x => x.CommentsToday)
            .Take(4)
            .ToListAsync();

            return CommentsCountWeek;
        }

        public async Task<User?> FavSubject(string userId)
        {
            var result = await _db.Users
                .Include(f => f.FavoriteSubjects)
                .Include(f => f.FavoriteThreads)
                .FirstOrDefaultAsync(p => p.Id == userId);
      

            return result;
        }
    
    }
}
