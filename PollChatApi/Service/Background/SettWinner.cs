using Microsoft.EntityFrameworkCore;
using PollChatApi.Model;

namespace PollChatApi.Service.Background
{
    public class SettWinner
    {
        private readonly MyDbContext _db;

        public SettWinner(MyDbContext db)
        { _db = db; }
        public async Task SetWinner()
        {

            var poll = await _db.Polls
                    .OrderByDescending(p => p.Id)
                    .FirstOrDefaultAsync();

            if (poll == null)
                return;

            var winner = await _db.Votes
                .Where(v => v.PollId == poll.Id)
                .GroupBy(v => v.SubjectId)
                .Select(g => new
                {
                    SubjectId = g.Key,
                    VoteCount = g.Count()
                })
                .OrderByDescending(g => g.VoteCount)
                .FirstOrDefaultAsync();

            if(winner != null)
            {
                poll.WinnerId = winner.SubjectId;
                await _db.SaveChangesAsync();
            }

        }
    }
}
