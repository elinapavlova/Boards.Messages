using Database.Repositories.Base;

namespace Database.Repositories.File
{
    public class FileRepository : BaseRepository, IFileRepository
    {
        public FileRepository(AppDbContext context) : base(context)
        {
        }
    }
}