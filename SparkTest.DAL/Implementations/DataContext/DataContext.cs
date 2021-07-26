using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Pluralize.NET;
using SparkTest.DAL.Interfaces.DataContext;

namespace SparkTest.DAL.Implementations.DataContext
{
    public class DataContext : IDataContext
    {
        IMongoDatabase database;

        public DataContext(IConfiguration config)
        {
            var client = new MongoClient(config["MongoConnection:ConnectionString"]);

            database = client.GetDatabase(config["MongoConnection:Database"]);
        }

        public DataContext(string connection, string databaseName)
        {
            var client = new MongoClient(connection);

            database = client.GetDatabase(databaseName);
        }

        public virtual IMongoCollection<TEntity> Get<TEntity>() where TEntity : class
        {
            var name = new Pluralizer().Pluralize(typeof(TEntity).Name);

            return database.GetCollection<TEntity>(name);
        }
    }
}
