using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boards.MessageService.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace Boards.MessageService.Database.Repositories.File
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;
        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<FileModel>> GetByMessageId(Guid id)
        {
            return _context.Set<FileModel>()
                .AsNoTracking()
                .AsEnumerable()
                .Where(f => f.MessageId == id)
                .ToList();
        }
        
        public async Task<FileModel> Create(FileModel file)
        {
            file.DateCreated = DateTime.Now;
            await _context.Set<FileModel>().AddAsync(file);
            await _context.SaveChangesAsync();
            return file;
        }
    }
}