using Core.Application.Repositories;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Core.Infraestructure.Repositories.MongoDb
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public DbContext Context { get; private set; }

        public IMongoCollection<TEntity> Collection { get; private set; }

        public BaseRepository(DbContext context)
        {
            Context = context;
            Collection = Context.GetCollection<TEntity>();
        }

        public object Add(TEntity entity)
        {
            Collection.InsertOne(entity);
            var property = typeof(TEntity).GetProperty("Id") ?? typeof(TEntity).GetProperty("_id");
            return (object)property?.GetValue(entity);
        }

        public async Task<object> AddAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
            var property = typeof(TEntity).GetProperty("Id") ?? typeof(TEntity).GetProperty("_id");
            return (object)property?.GetValue(entity);
        }

        public long Count(Expression<Func<TEntity, bool>> filter)
        {
            return Collection.CountDocuments(filter);
        }

        public async Task<long> CountAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await Collection.CountDocumentsAsync(filter);
        }
        
        public List<TEntity> FindAll()
        {
            FilterDefinition<TEntity> filter = new BsonDocumentFilterDefinition<TEntity>([]);
            return Collection.Find(filter).ToList();
        }

        public async Task<List<TEntity>> FindAllAsync()
        {
            FilterDefinition<TEntity> filter = new BsonDocumentFilterDefinition<TEntity>([]);
            return await Collection.Find(filter).ToListAsync();
        }

        public TEntity FindOne(params object[] keyValues)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", keyValues[0]);
            return Collection.Find(filter).SingleOrDefault();
        }

        public async Task<TEntity> FindOneAsync(params object[] keyValues)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", keyValues[0]);
            return await Collection.Find(filter).SingleOrDefaultAsync();
        }

        public void Remove(params object[] keyValues)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", keyValues[0]);
            Collection.DeleteOne(filter);
        }

        public void Update(object id, TEntity entity)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("_id", id);
            Collection.ReplaceOne(filter, entity);
        }
    }
}
