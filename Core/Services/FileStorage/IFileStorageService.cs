using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Result;
using Core.Dto.File.Upload;
using Database.Models;
using Microsoft.AspNetCore.Http;

namespace Core.Services.FileStorage
{
    public interface IFileStorageService
    {
        Task<ResultContainer<UploadFilesResponseDto>> Upload(IFormFileCollection files,Guid messageId, Guid threadId);
        Task<ICollection<FileModel>> GetByMessageId(Guid id);
    }
}