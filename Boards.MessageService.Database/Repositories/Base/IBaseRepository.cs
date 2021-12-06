using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Boards.Common.Base;

namespace Boards.MessageService.Database.Repositories.Base
{
    public interface IBaseRepository
    {
        Task<TModel> Create<TModel>(TModel item) where TModel : BaseModel;
        List<TEntity> Get<TEntity>(Func<TEntity, bool> predicate) where TEntity : BaseModel;
        Task<TEntity> GetById<TEntity>(Guid id) where TEntity : BaseModel;
        Task<TEntity> Remove<TEntity>(Guid id) where TEntity : BaseModel;
    }
}