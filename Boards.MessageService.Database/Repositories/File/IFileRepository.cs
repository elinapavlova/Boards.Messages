using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.MessageService.Database.Models;

namespace Boards.MessageService.Database.Repositories.File
{
    public interface IFileRepository
    {
        Task<List<FileModel>> GetByMessageId(Guid id);
        Task<FileModel> Create(FileModel file);
    }
}