using Microsoft.EntityFrameworkCore;
using PollChatApi.Model;

namespace PollChatApi.Service.Background
{
    public class DeleteFromDatabase
    {
        private readonly MyDbContext _db;

        public DeleteFromDatabase(MyDbContext db)
        {
            _db = db;
        }

        public async Task DeleteDailyFromDatabase()
        {
            DateTime now = DateTime.UtcNow.AddDays(-30);

            var oldThreads = await _db.MainThreads
                 .Include(t => t.Comments)
                 .Where(t => t.RemovedAt != null && t.RemovedAt <= now)
                 .ToListAsync();

            foreach (var item in oldThreads)
            {
                if(!string.IsNullOrEmpty(item.ImagePath))
                {
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/userImages", item.ImagePath);

                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                    }
                }

                foreach(var comment in item.Comments)
                {
                    _db.Remove(comment);
                }
                _db.MainThreads.Remove(item);
            }

            await _db.SaveChangesAsync();
        }
    }
}
