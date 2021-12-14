using System;
using System.Threading.Tasks;
using Boards.MessageService.Database.Models;

namespace Boards.MessageService.Database.Repositories.Thread
{
    public interface IThreadRepository
    {
        Task<ThreadModel> GetById(Guid id);
    }
}