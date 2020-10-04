using MongoDBWebApi.Database.Models.Base;
using MongoDBWebApi.Database.Settings;

namespace MongoDBWebApi.Database.Models
{
    [BsonCollection("movie")]
    public class Movie : Entity
    {
        public string name { get; set; }
    }
}