using Microsoft.EntityFrameworkCore;
using PollChatApi.DTO;

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

        public DbSet<ThreadEditHistory> ThreadHistory { get; set; }

        public DbSet<CommentEditHistory> CommentHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MainThread>()
                .HasQueryFilter(t => t.RemovedAt == null);

            modelBuilder.Entity<Comment>()
                .HasQueryFilter(c => c.RemovedAt == null);

        }

    }
}
