using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Boards.MessageService.Core.Dto.File;
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

        public async Task<ICollection<FileResultDto>> GetByMessageId(Guid id)
        {
            using var client = _clientFactory.CreateClient("FileStorage");
            var response = await client.PostAsync($"By-Message-Id/{id}", null!);
            var responseMessage = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ICollection<FileResultDto>>(responseMessage);
            return result;
        }
        
        public async Task<List<FileResponseDto>> Upload(IFormFileCollection files, Guid messageId,
            Guid threadId)
        {

            var content = await CreateContent(files);
            if (content == null)
                return null;

            using var client = _clientFactory.CreateClient("FileStorage");
            var response = await client.PostAsync("Upload", content);
            var responseMessage = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<FileResponseDto>>(responseMessage);

            if (result == null)
                return null;

            await AddFilesToDatabase(threadId, messageId, result);

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
                multiContent.Add(streamContent, file.Name, file.FileName);
            }

            return multiContent;
        }
        
        private async Task AddFilesToDatabase(Guid threadId, Guid messageId, IEnumerable<FileResponseDto> files)
        {
            foreach (var file in files)
            {
                var newFile = _mapper.Map<FileModel>(file);
                newFile.DateCreated = DateTime.Now;
                newFile.ThreadId = threadId;
                newFile.MessageId = messageId;
                
                await _fileRepository.Create(newFile);
            }
        }
    }
}