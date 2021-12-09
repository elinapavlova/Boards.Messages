using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.MessageService.Core.Dto.File;
using Microsoft.AspNetCore.Http;

namespace Boards.MessageService.Core.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<List<FileResponseDto>> Upload(IFormFileCollection files, Guid messageId, Guid threadId);
        Task<ICollection<FileResultDto>> GetByMessageId(Guid id);
    }
}