using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.MessageService.Database.Models;
using Boards.MessageService.Database.Repositories.Base;

namespace Boards.MessageService.Database.Repositories.Message
{
    public interface IMessageRepository : IBaseRepository
    {
        Task<ICollection<MessageModel>> GetByThreadId(Guid id, int pageNumber, int pageSize);
    }
}