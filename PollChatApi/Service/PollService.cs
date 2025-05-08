using Microsoft.EntityFrameworkCore;
using PollChatApi.Model;

namespace PollChatApi.Service
{
    public class PollService
    {
        private readonly MyDbContext _db;

        public PollService(MyDbContext db)
        {
            _db = db;
        }

        public async Task CreateNewPollAsync()
        {
            if (DateTime.Today.DayOfWeek != DayOfWeek.Monday)
            {
                return;
            }

            bool pollExist = await _db.Polls.AnyAsync(p => EF.Functions.DateDiffWeek(p.Date, DateTime.Today) == 0);

            if (pollExist)
            {
                return;
            }

            var subject = await _db.Subjects.OrderBy(s => Guid.NewGuid()).Take(4).ToListAsync();

            if (subject.Count < 1)
            {
                throw new Exception("Not enough subjects to make an poll draft");
            }

            var newpoll = new Poll
            {
                Date = DateTime.Today,
                Subjects = subject,
            };

            _db.Polls.Add(newpoll);
            _db.SaveChanges();
        }

        public async Task SettWinner(int pollId)
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

        public async Task<List<PollResultDto>> CountVotes(int pollId)
        {
            var poll = await _db.Polls.FindAsync(pollId);

            if (poll == null)
            {
                throw new Exception("Poll not found");
            }

            var results = await _db.Votes.Where(v => v.PollId == pollId).GroupBy
                             (v => v.SubjectId).Select(s =>
                             new PollResultDto
                             {
                                 SubjectId = s.Key,
                                 Votes = s.Count(),
                                 Title = _db.Subjects.FirstOrDefault(g => g.Id == s.Key).Title
                             })
                             .ToListAsync();
            return results;
        }
    }
}
