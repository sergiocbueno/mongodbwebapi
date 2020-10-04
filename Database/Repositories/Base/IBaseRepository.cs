using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDBWebApi.Database.Models.Base;

namespace MongoDBWebApi.Database.Repositories.Base
{
    public interface IBaseRepository<TEntity> where TEntity : IEntity
    {
        IQueryable<TEntity> GetAll();

        IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> filterExpression);

        IEnumerable<TProjected> GetBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression);

        Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filterExpression);

        TEntity GetById(string id);

        Task<TEntity> GetByIdAsync(string id);

        void Save(TEntity document);

        Task SaveAsync(TEntity document);

        void SaveMany(ICollection<TEntity> documents);

        Task SaveManyAsync(ICollection<TEntity> documents);

        void Update(TEntity document);

        Task UpdateAsync(TEntity document);

        void Delete(Expression<Func<TEntity, bool>> filterExpression);

        Task DeleteAsync(Expression<Func<TEntity, bool>> filterExpression);

        void DeleteById(string id);

        Task DeleteByIdAsync(string id);

        void DeleteMany(Expression<Func<TEntity, bool>> filterExpression);

        Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression);
    }
}