using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Boards.MessageService.Core.Dto.File;
using Boards.MessageService.Core.Options;
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
        private readonly Uri _fileStorageBaseAddress;

        public FileStorageService
        (
            IHttpClientFactory clientFactory,
            IFileRepository fileRepository,
            IMapper mapper,
            BaseAddresses addresses
        )
        {
            _clientFactory = clientFactory;
            _fileRepository = fileRepository;
            _mapper = mapper;
            _fileStorageBaseAddress = new Uri(addresses.FileStorage);
        }

        public async Task<Collection<Uri>> GetByMessageId(Guid id)
        {
            var result = new Collection<Uri>();
            var files = await _fileRepository.GetByMessageId(id);
            if (files == null)
                return null;
            
            foreach (var file in files)
                result.Add(CreateUrl(file.Path));

            return result;
        }
        
        private Uri CreateUrl(string path)
        {
            var name = Path.GetFileName(path);
            var url = new Uri(_fileStorageBaseAddress, name);
            return url;
        }
        
        public async Task<List<FileResponseDto>> Upload(IFormFileCollection files, Guid messageId, Guid threadId)
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

            foreach (var file in result)
                file.Url = CreateUrl(file.Path);
            
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