using Boards.MessageService.Database.Repositories.Base;

namespace Boards.MessageService.Database.Repositories.Thread
{
    public class ThreadRepository : BaseRepository, IThreadRepository
    {
        public ThreadRepository(AppDbContext context) : base(context)
        {
        }
    }
}