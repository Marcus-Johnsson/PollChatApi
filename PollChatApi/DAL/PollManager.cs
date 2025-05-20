using Microsoft.EntityFrameworkCore;
using PollChatApi.DTO;
using PollChatApi.Model;


namespace PollChatApi.DAL
{
    public static class PollManager
    {
        private static MyDbContext _db;

        public static void init(MyDbContext db) 
        {
            _db = db;
        }

        public static async Task<List<PollResultDto>> CountCurrentPoll()
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


        public static async Task CreateNewPollAsync()
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
