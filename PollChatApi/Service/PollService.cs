using Microsoft.EntityFrameworkCore;
using PollChatApi.Model;
using PollChatApi.Models;
using SubjectWars.Data.Dto;

namespace PollChatApi.Service
{
    public class PollServices
    {
        private readonly MyDbContext _db;

        public PollServices(MyDbContext db)
        {
            _db = db;
        }





        public async Task SetWinner(int pollId)
        {
            var poll = await _db.Polls.FindAsync(pollId);
            if (poll == null)
            {
                throw new Exception("Poll not found");
            }
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
            {
                var finishiedPoll = await _db.Votes.Where(v => v.PollId == pollId).GroupBy
                                             (v => v.SubjectId).Select(s =>
                                             new
                                             {
                                                 SubjectId = s.Key,
                                                 VoteCount = s.Count()
                                             })
                                             .OrderByDescending(x => x.VoteCount)
                                             .ToArrayAsync();


                if (finishiedPoll.Any())
                {
                    var winnerId = finishiedPoll.First().SubjectId;
                    poll.WinnerId = winnerId;
                    await _db.SaveChangesAsync();
                }
            }

        }

        public async Task<List<PollResultDto>> CountCurrentPoll()
        {
            var latestPoll = await _db.Polls
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            if (latestPoll == null)
            {
                throw new Exception("Poll not found");
            }

            var results = await _db.Votes
                    .Where(v => v.PollId == latestPoll.Id)
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
                    .ToListAsync();
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

        public async Task<List<CommentCount>> MostCommentsToday()
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

        public async Task<List<CommentCount>> MostCommentsWeek()
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

        public async Task<List<Subject>> FavSubject(string userId)
        {
            var result = _db.Users
                .Include(f=>f.)
        }
    }
}
