using Domain.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Infrastructure.Repositories.Mongo.Maps
{
    internal static class DummyEntityMap
    {
        public static void Configure()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(DummyEntity)))
            {
                BsonClassMap.RegisterClassMap((BsonClassMap<DummyEntity> c) =>
                {
                    c.AutoMap();
                    c.SetIgnoreExtraElements(true);
                });
            }
        }

        public static string GetCollectionName()
        {
            return "common.dummyentity";
        }
    }
}
