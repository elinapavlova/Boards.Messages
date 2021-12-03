﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using Common.Error;
using Common.Result;
using Core.Dto.File;
using Core.Dto.File.Upload;
using Core.Dto.Message;
using Core.Dto.Message.Create;
using Core.Services.FileStorage;
using Database.Models;
using Database.Repositories.Message;
using Database.Repositories.Thread;

namespace Core.Services.Message
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IThreadRepository _threadRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IMapper _mapper;

        public MessageService
        (
            IMessageRepository messageRepository, 
            IThreadRepository threadRepository,
            IFileStorageService fileStorageService,
            IMapper mapper
        )
        {
            _messageRepository = messageRepository;
            _threadRepository = threadRepository;
            _fileStorageService = fileStorageService;
            _mapper = mapper;
        }

        public async Task<ResultContainer<CreateMessageResponseDto>> Create(CreateMessageRequestDto data)
        {
            var result = new ResultContainer<CreateMessageResponseDto>();
            var resultUpload = new ResultContainer<UploadFilesResponseDto>();
            
            if (string.IsNullOrEmpty(data.Text))
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            var thread = await _threadRepository.GetById<ThreadModel>(data.ThreadId);
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

            if (resultUpload.Data != null)
                result.Data.Files = resultUpload.Data.Files;

            if (resultUpload.ErrorType.HasValue)
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
            result.Data.Files = _mapper.Map<ICollection<FileResponseDto>>(await _fileStorageService.GetByMessageId(id));
            return result;
        }

        public async Task<ResultContainer<ICollection<MessageModelDto>>> GetByThreadId(Guid id)
        {
            var messages = _messageRepository.Get<MessageModel>(m => m.ThreadId == id);
            var result = _mapper.Map<ResultContainer<ICollection<MessageModelDto>>>(messages);
            foreach (var message in result.Data)
            {
                message.Files = _mapper.Map<ICollection<FileResponseDto>>(await _fileStorageService.GetByMessageId(message.Id));
                message.ReferenceToMessage = message.ReferenceToMessage.Id == Guid.Empty ? null : 
                    _mapper.Map<ReferenceToMessageDto>(await _messageRepository.GetById<MessageModel>(message.ReferenceToMessage.Id));
            }
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