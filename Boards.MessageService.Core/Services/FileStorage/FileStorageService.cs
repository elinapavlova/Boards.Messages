using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Boards.MessageService.Core.Dto.File;
using Boards.MessageService.Core.Dto.File.Upload;
using Boards.Common.Error;
using Boards.Common.Result;
using Boards.MessageService.Database.Models;
using Boards.MessageService.Database.Repositories.File;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Boards.MessageService.Core.Services.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;

        public FileStorageService
        (
            IHttpClientFactory clientFactory,
            IFileRepository fileRepository,
            IMapper mapper
        )
        {
            _clientFactory = clientFactory;
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        public async Task<ICollection<FileModel>> GetByMessageId(Guid id)
        {
            var files = _fileRepository.Get<FileModel>(f => f.MessageId != null && f.MessageId == id);
            return files;
        }
        
        public async Task<ResultContainer<UploadFilesResponseDto>> Upload(IFormFileCollection files, Guid messageId, Guid threadId)
        {
            var result = new ResultContainer<UploadFilesResponseDto>();
            var content = await CreateContent(files);
            if (content == null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            using var client = _clientFactory.CreateClient("FileStorage");
            var response = await client.PostAsync("Upload", content);
            var responseMessage = await response.Content.ReadAsStringAsync();
            var responseJson = JsonConvert.DeserializeObject<UploadFilesResultDto>(responseMessage);

            result = await ValidateResult(responseJson, messageId, threadId);
            return result;
        }
        
        private static async Task<MultipartFormDataContent> CreateContent(IFormFileCollection files)
        {
            var multiContent = new MultipartFormDataContent();

            if (files.Count > 10)
                return null;
            
            foreach (var file in files)
            {
                // Если файл пустой или его размер больше 100 мб
                if (file.Length is <= 0 or > 104857600)
                    return null;

                var streamContent = new StreamContent(file.OpenReadStream());
                streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                streamContent.Headers.Add("api-version", "1.0");
                multiContent.Add(streamContent, file.Name, file.FileName);
            }

            return multiContent;
        }
        
        private async Task<ResultContainer<UploadFilesResponseDto>> ValidateResult
            (UploadFilesResultDto files, Guid messageId, Guid threadId)
        {
            var result = new ResultContainer<UploadFilesResponseDto>
            {
                Data = new UploadFilesResponseDto
                {
                    Files = new List<FileResponseDto>()
                }
            };

            if (files.ErrorType != null)
            {
                result.ErrorType = ErrorType.BadRequest;
                return result;
            }

            foreach (var file in files.Data)
            {
                file.ThreadId = threadId;
                file.MessageId = messageId;
                switch (file.Extension)
                {
                    case ".mp4" :
                    case ".wav" :
                    case ".txt" :
                    case ".png" :
                    case ".jpg" :
                        var res = await AddFileToDatabase(file);
                        result.Data.Files.Add(res);
                        break;
                }
            }

            return result;
        }

        private async Task<FileResponseDto> AddFileToDatabase(FileResponseDto newFile)
        {
            var file = _mapper.Map<FileModel>(newFile);
            var result = _mapper.Map<FileResponseDto>(await _fileRepository.Create(file));

            return result;
        }
    }
}