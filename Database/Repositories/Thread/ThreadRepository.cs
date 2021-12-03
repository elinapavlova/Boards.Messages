using Database.Repositories.Base;

namespace Database.Repositories.Thread
{
    public class ThreadRepository : BaseRepository, IThreadRepository
    {
        public ThreadRepository(AppDbContext context) : base(context)
        {
        }
    }
}