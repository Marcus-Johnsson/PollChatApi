using Microsoft.EntityFrameworkCore;

namespace PollChatApi.Model
{
    public class MyDbContext : DbContext
    {

        public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
        { }

        public DbSet<Poll> Polls { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Vote> Votes { get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<MainThread> MainThreads { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<SubCategory> SubCategory { get; set; }

    }
}
