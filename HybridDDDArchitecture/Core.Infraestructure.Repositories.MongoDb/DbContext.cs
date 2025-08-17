using MongoDB.Driver;

namespace Core.Infraestructure.Repositories.MongoDb
{
    public abstract class DbContext
    {
        protected IMongoDatabase Database;

        public abstract IMongoCollection<TEntity> GetCollection<TEntity>();

        protected DbContext(string connectionString)
        {
            MongoClientSettings settings =  MongoClientSettings.FromConnectionString(connectionString ?? throw new ArgumentNullException(nameof(connectionString)));

            MongoUrl url = new MongoUrl(connectionString ?? throw new ArgumentNullException(nameof(connectionString)));
            MongoClient client = new MongoClient(url);
            Database = client.GetDatabase(url.DatabaseName);
        }
    }
}
