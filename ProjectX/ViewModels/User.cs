using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ProjectX.Views
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string username { get; set; }
        public string password { get; set; }
        public string apikey { get; set; }
        public bool __v { get; set; }
    }
}