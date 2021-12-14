using System;
using System.Threading.Tasks;
using Boards.Auth.Common.Base;

namespace Boards.MessageService.Database.Repositories.Base
{
    public interface IBaseRepository
    {
        Task<TModel> Create<TModel>(TModel item) where TModel : BaseModel;
        Task<TEntity> GetById<TEntity>(Guid id) where TEntity : BaseModel;
        Task<TEntity> Remove<TEntity>(Guid id) where TEntity : BaseModel;
    }
}