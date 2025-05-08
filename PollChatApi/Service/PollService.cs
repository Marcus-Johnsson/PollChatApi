using Microsoft.EntityFrameworkCore;
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
