using MongoDB.Bson.Serialization.Attributes;
using SparkTest.DAL.Domain.Interfaces;
using System;

namespace SparkTest.DAL.Domain.Entities
{
    public class ApplicationUser : IEntity<string>
    {
        [BsonId]
        public string Id { get; set; }

        public string Name { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
