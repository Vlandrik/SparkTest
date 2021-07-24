using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SparkTest.DAL.Domain.Interfaces;
using System;

namespace SparkTest.DAL.Domain.Entities
{
    public class ApplicationUser : IEntity<string>
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
