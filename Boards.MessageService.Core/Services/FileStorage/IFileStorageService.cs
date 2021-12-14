using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Boards.MessageService.Core.Dto.File;
using Microsoft.AspNetCore.Http;

namespace Boards.MessageService.Core.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<List<FileResponseDto>> Upload(IFormFileCollection files, Guid messageId, Guid threadId);
        Task<Collection<Uri>> GetByMessageId(Guid id);
    }
}