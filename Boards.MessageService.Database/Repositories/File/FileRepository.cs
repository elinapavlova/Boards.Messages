using Boards.MessageService.Database.Repositories.Base;

namespace Boards.MessageService.Database.Repositories.File
{
    public class FileRepository : BaseRepository, IFileRepository
    {
        public FileRepository(AppDbContext context) : base(context)
        {
        }
    }
}