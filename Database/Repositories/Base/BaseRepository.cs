using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBWebApi.Database.Models.Base;
using MongoDBWebApi.Database.Settings;

namespace MongoDBWebApi.Database.Repositories.Base
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : IEntity
    {
        private readonly IMongoCollection<TEntity> _collection;

        public BaseRepository(IMongoDbSettings settings)
        {
            var database = new MongoClient(settings.ConnectionString).GetDatabase(settings.DatabaseName);
            _collection = database.GetCollection<TEntity>(GetCollectionName(typeof(TEntity)));
        }

        private protected string GetCollectionName(Type documentType)
        {
            return ((BsonCollectionAttribute) documentType.GetCustomAttributes(
                    typeof(BsonCollectionAttribute),
                    true)
                .FirstOrDefault())?.CollectionName;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return _collection.AsQueryable();
        }

        public virtual IEnumerable<TEntity> GetBy(Expression<Func<TEntity, bool>> filterExpression)
        {
            return _collection.Find(filterExpression).ToEnumerable();
        }

        public virtual IEnumerable<TProjected> GetBy<TProjected>(Expression<Func<TEntity, bool>> filterExpression, Expression<Func<TEntity, TProjected>> projectionExpression)
        {
            return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
        }

        public virtual Task<TEntity> GetByAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => _collection.Find(filterExpression).FirstOrDefaultAsync());
        }

        public virtual TEntity GetById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            return _collection.Find(filter).SingleOrDefault();
        }

        public virtual Task<TEntity> GetByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
                return _collection.Find(filter).SingleOrDefaultAsync();
            });
        }


        public virtual void Save(TEntity document)
        {
            _collection.InsertOne(document);
        }

        public virtual Task SaveAsync(TEntity document)
        {
            return Task.Run(() => _collection.InsertOneAsync(document));
        }

        public void SaveMany(ICollection<TEntity> documents)
        {
            _collection.InsertMany(documents);
        }


        public virtual async Task SaveManyAsync(ICollection<TEntity> documents)
        {
            await _collection.InsertManyAsync(documents);
        }

        public void Update(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            _collection.FindOneAndReplace(filter, document);
        }

        public virtual async Task UpdateAsync(TEntity document)
        {
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, document.Id);
            await _collection.FindOneAndReplaceAsync(filter, document);
        }

        public void Delete(Expression<Func<TEntity, bool>> filterExpression)
        {
            _collection.FindOneAndDelete(filterExpression);
        }

        public Task DeleteAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => _collection.FindOneAndDeleteAsync(filterExpression));
        }

        public void DeleteById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
            _collection.FindOneAndDelete(filter);
        }

        public Task DeleteByIdAsync(string id)
        {
            return Task.Run(() =>
            {
                var objectId = new ObjectId(id);
                var filter = Builders<TEntity>.Filter.Eq(doc => doc.Id, objectId);
                _collection.FindOneAndDeleteAsync(filter);
            });
        }

        public void DeleteMany(Expression<Func<TEntity, bool>> filterExpression)
        {
            _collection.DeleteMany(filterExpression);
        }

        public Task DeleteManyAsync(Expression<Func<TEntity, bool>> filterExpression)
        {
            return Task.Run(() => _collection.DeleteManyAsync(filterExpression));
        }
    }
}