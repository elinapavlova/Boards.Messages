using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.MessageService.Core.Dto.File.Upload;
using Boards.Common.Result;
using Boards.MessageService.Database.Models;
using Microsoft.AspNetCore.Http;

namespace Boards.MessageService.Core.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<ResultContainer<UploadFilesResponseDto>> Upload(IFormFileCollection files,Guid messageId, Guid threadId);
        Task<ICollection<FileModel>> GetByMessageId(Guid id);
    }
}