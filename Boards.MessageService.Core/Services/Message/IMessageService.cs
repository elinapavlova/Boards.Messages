using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.MessageService.Core.Dto.Message;
using Boards.MessageService.Core.Dto.Message.Create;
using Common.Filter;
using Common.Result;

namespace Boards.MessageService.Core.Services.Message
{
    public interface IMessageService
    {
        Task<ResultContainer<MessageModelDto>> GetById(Guid id);
        Task<ResultContainer<CreateMessageResponseDto>> Create(CreateMessageRequestDto data);
        Task<ResultContainer<ICollection<MessageModelDto>>> GetByThreadId(Guid id, FilterPagingDto filter);
        Task<ResultContainer<MessageModelResponseDto>> Delete(Guid id);
    }
}