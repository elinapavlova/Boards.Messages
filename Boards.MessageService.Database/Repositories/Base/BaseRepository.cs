using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boards.Auth.Common.Base;
using Microsoft.EntityFrameworkCore;

namespace Boards.MessageService.Database.Repositories.Base
{
    public class BaseRepository : IBaseRepository
    {
        private readonly AppDbContext _context;

        protected BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TModel> Create<TModel>(TModel item) where TModel : BaseModel
        {
            item.DateCreated = DateTime.Now;
            await _context.Set<TModel>().AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }
        
        public List<TEntity> Get<TEntity>(Func<TEntity, bool> predicate) where TEntity : BaseModel
        {
            return _context.Set<TEntity>().AsNoTracking().AsEnumerable().Where(predicate).ToList();
        }

        public async Task<TEntity> GetById<TEntity>(Guid id) where TEntity : BaseModel
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Remove<TEntity>(Guid id) where TEntity : BaseModel
        {
            var item = await _context.Set<TEntity>().FindAsync(id);
            if (item == null)
                return null;
            
            _context.Set<TEntity>().Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }
    }
}