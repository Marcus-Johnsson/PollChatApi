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
    }
}
