using Boards.MessageService.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Boards.MessageService.Database
{
    public class AppDbContext : DbContext
    {
        public DbSet<ThreadModel> Threads { get; set; }
        public DbSet<MessageModel> Messages { get; set; }
        public DbSet<FileModel> Files { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<MessageModel>(message =>
            {
                message.Property(m => m.Text).IsRequired().HasMaxLength(500);
                message.Property(m => m.DateCreated).IsRequired();
                
                message.HasOne(m => m.Thread)
                    .WithMany(t => t.Messages)
                    .HasForeignKey(m => m.ThreadId);
            });

            builder.Entity<FileModel>(file =>
            {
                file.HasOne(f => f.Message)
                    .WithMany(m => m.Files)
                    .HasForeignKey(f => f.MessageId);
            });
        }
    }
}