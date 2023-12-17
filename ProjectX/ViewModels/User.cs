using System;
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
        public string zaloapi { get; set; }

        public string fptapi { get; set; }

        public bool __v { get; set; }

        public DateTime last_used_at;

        public DateTime created_at;
    }
}