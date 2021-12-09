using System;
using System.Threading.Tasks;
using Boards.MessageService.Core.Dto.Thread;

namespace Boards.MessageService.Core.Services.Thread
{
    public interface IThreadService
    {
        Task<ThreadResponseDto> GetById(Guid id);
    }
}