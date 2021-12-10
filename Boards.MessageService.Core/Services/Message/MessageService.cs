using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Boards.Auth.Common.Error;
using Boards.Auth.Common.Filter;
using Boards.Auth.Common.Result;
using Boards.MessageService.Core.Dto.Message;
using Boards.MessageService.Core.Dto.Message.Create;
using Boards.MessageService.Core.Services.FileStorage;
using Boards.MessageService.Database.Models;
using Boards.MessageService.Database.Repositories.Message;
using Boards.MessageService.Core.Dto.File;
using Boards.MessageService.Core.Services.Thread;

namespace Boards.MessageService.Core.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IThreadService _threadService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public MessageService
        (
            IMessageRepository messageRepository, 
            IThreadService threadService,
            IFileStorageService fileStorageService,
            IMapper mapper
        )
        {
            _messageRepository = messageRepository;
            _threadService = threadService;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<ResultContainer<CreateMessageResponseDto>> Create(CreateMessageRequestDto data)
        {
            var result = new ResultContainer<CreateMessageResponseDto>();
            var resultUpload = new List<FileResponseDto>();
            
            if (string.IsNullOrEmpty(data.Text))
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var thread = await _threadService.GetById(data.ThreadId);
            if (thread == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result; 
            }

            if (data.ReferenceToMessage != null)
            {
                var referenceToMessage = await _messageRepository.GetById<MessageModel>((Guid) data.ReferenceToMessage);
                if (referenceToMessage == null || data.ThreadId != referenceToMessage.ThreadId)
                {
                    result.ErrorType = ErrorType.NotFound;
                    return result;
                }
            }
            var message = _mapper.Map<MessageModel>(data);

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            result = _mapper.Map<ResultContainer<CreateMessageResponseDto>>(await _messageRepository.Create(message));
            
            if (data.Files != null)
                resultUpload = await _fileStorageService.Upload(data.Files, result.Data.Id, result.Data.ThreadId);

            if (resultUpload != null)
                foreach (var file in resultUpload)
                    result.Data.Files.Add(file.Url);

            if (resultUpload == null)
                result.ErrorType = ErrorType.BadRequest;
            else 
                scope.Complete();

            return result;
        }

        public async Task<ResultContainer<MessageModelDto>> GetById(Guid id)
        {
            MessageModel referenceToMessage = null;
            var result = new ResultContainer<MessageModelDto>
            {
                Data = new MessageModelDto()
            };

            var message = await _messageRepository.GetById<MessageModel>(id);
            if (message == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            }

            if (message.ReferenceToMessage != null)
            {
                referenceToMessage = await _messageRepository.GetById<MessageModel>((Guid) message.ReferenceToMessage);
                if (referenceToMessage == null)
                {
                    result.ErrorType = ErrorType.NotFound;
                    return result;
                }
            }

            result = _mapper.Map<ResultContainer<MessageModelDto>>(message);
            result.Data.ReferenceToMessage = _mapper.Map<ReferenceToMessageDto>(referenceToMessage);
            result.Data.Files = await _fileStorageService.GetByMessageId(id);
            return result;
        }

        public async Task<ResultContainer<ICollection<MessageModelDto>>> GetByThreadId(Guid id, FilterPagingDto filter)
        {
            var result = new ResultContainer<ICollection<MessageModelDto>>();
            var messages = await _messageRepository.GetByThreadId(id, filter.PageNumber, filter.PageSize);
            if (messages == null)
            {
                result.ErrorType = ErrorType.NotFound;
                return result;
            } 
            result = _mapper.Map<ResultContainer<ICollection<MessageModelDto>>>(messages);
            
            foreach(var file in result.Data)
                file.Files = await _fileStorageService.GetByMessageId(file.Id);
            return result;
        }

        public async Task<ResultContainer<MessageModelResponseDto>> Delete(Guid id)
        {
            var result = new ResultContainer<MessageModelResponseDto>();
            var message = await _messageRepository.Remove<MessageModel>(id);
            if (message != null) 
                return _mapper.Map<ResultContainer<MessageModelResponseDto>>(message);
            
            result.ErrorType = ErrorType.NotFound;
            return result;
        }
    }
}