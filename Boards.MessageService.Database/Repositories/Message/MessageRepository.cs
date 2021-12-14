using Boards.MessageService.Database.Repositories.Base;

namespace Boards.MessageService.Database.Repositories.Message
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
        }
    }
}