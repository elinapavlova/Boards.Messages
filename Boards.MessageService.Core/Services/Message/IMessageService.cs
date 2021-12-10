using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.Auth.Common.Filter;
using Boards.Auth.Common.Result;
using Boards.MessageService.Core.Dto.Message;
using Boards.MessageService.Core.Dto.Message.Create;

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