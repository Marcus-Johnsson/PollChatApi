using Microsoft.EntityFrameworkCore;
using PollChatApi.Model;

namespace PollChatApi.Service
{
    public interface IPollHandler
    {
        Task CreateWeeklyPollAsync();
        Task SetWinner(int pollId);
        Task<List<PollResultDto>> CountCurrentPoll();
    }

        public class PollHandler : IPollHandler
    {
        private readonly MyDbContext _db;
        private readonly PollServices _services;

        public PollHandler(MyDbContext db, PollServices pollServices)
        {
            _db = db;
            _services = pollServices;
        }

        public async Task SetWinner(int pollId)
        {
            await _services.SetWinner(pollId);
        }

        public async Task<List<PollResultDto>> CountCurrentPoll()
        {
            return await _services.CountCurrentPoll();
        }


        public async Task CreateWeeklyPollAsync()
        {
            await CreateNewPollAsync();
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