using PollChatApi.Model;
using Microsoft.EntityFrameworkCore;

namespace PollChatApi.Service.Background
{
    public class CreatePollWeekly
    {
        private readonly MyDbContext _db;

        public CreatePollWeekly(MyDbContext db)
        {
            _db = db;
        }

        public async Task CreateNewPollAsync()
        {
            var newestPoll = await _db.Polls
                .OrderByDescending(p => p.Id)
                .FirstOrDefaultAsync();

            if (DateTime.Today.DayOfWeek == DayOfWeek.Monday)
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
    }
}
