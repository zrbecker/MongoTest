using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace GuestBook
{
    public class MongoGuestBook : IGuestBook
    {
        private readonly IMongoDatabase db;
        private const string collectionName = "guestbook";

        static MongoGuestBook()
        {
            BsonClassMap.RegisterClassMap<GuestBookEntry>(cm =>
            {
                cm.MapMember(c => c.Name);
                cm.MapMember(c => c.Message);
            });
        }

        public MongoGuestBook(IMongoDatabase database)
        {
            db = database;
        }

        public int Count
        {
            get
            {
                try
                {
                    var cursor = db.GetCollection<GuestBookEntry>(collectionName).Find(new BsonDocument());
                    return (int)cursor.CountAsync().Result;
                }
                catch (Exception ex)
                {
                    throw new GuestbookException("Failed to count entries in guest book", ex);
                }
            }
        }

        public void Post(GuestBookEntry entry)
        {
            try
            {
                db.GetCollection<GuestBookEntry>(collectionName).InsertOneAsync(entry).Wait();
            }
            catch (Exception ex)
            {
                throw new GuestbookException("Failed to post to guest book", ex);
            }
        }

        public IEnumerable<GuestBookEntry> Read(int index, int count)
        {
            try
            {
                var cursor = db.GetCollection<GuestBookEntry>(collectionName).Find(new BsonDocument());
                cursor.Skip(index);
                cursor.Limit(count);
                return cursor.ToListAsync().Result;
            }
            catch (Exception ex)
            {
                throw new GuestbookException("Failed to read from guest book", ex);
            }
        }
    }
}
