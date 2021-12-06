using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boards.MessageService.Database.Models;
using Boards.MessageService.Database.Repositories.Base;

namespace Boards.MessageService.Database.Repositories.Message
{
    public class MessageRepository : BaseRepository, IMessageRepository
    {
        public MessageRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<ICollection<MessageModel>> GetByThreadId(Guid id, int pageNumber, int pageSize)
        {
            var messages = Get<MessageModel>(m => m.ThreadId == id)
                .OrderByDescending(b => b.DateCreated)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            if (messages.Count == 0)
                return null;

            foreach (var message in messages)
                message.Files = Get<FileModel>(f => f.ThreadId == message.ThreadId && f.MessageId == message.Id)
                    .AsEnumerable().ToList();

            return messages;
        }
    }
}