using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Result;
using Core.Dto.Message;
using Core.Dto.Message.Create;

namespace Core.Services.Message
{
    public interface IMessageService
    {
        Task<ResultContainer<MessageModelDto>> GetById(Guid id);
        Task<ResultContainer<CreateMessageResponseDto>> Create(CreateMessageRequestDto data);
        Task<ResultContainer<ICollection<MessageModelDto>>> GetByThreadId(Guid id);
        Task<ResultContainer<MessageModelResponseDto>> Delete(Guid id);
    }
}