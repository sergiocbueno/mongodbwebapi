using System;
using MongoDB.Bson;

namespace MongoDBWebApi.Database.Models.Base
{
    public abstract class Entity : IEntity
    {
        public ObjectId Id { get; set; }

        public DateTime CreatedAt => Id.CreationTime;
    }
}