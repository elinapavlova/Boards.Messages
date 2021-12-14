using System;
using System.Threading.Tasks;
using Boards.MessageService.Database.Models;

namespace Boards.MessageService.Database.Repositories.Thread
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly AppDbContext _context;
        public ThreadRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ThreadModel> GetById(Guid id)
        {
            return await _context.Set<ThreadModel>().FindAsync(id);
        }
    }
}