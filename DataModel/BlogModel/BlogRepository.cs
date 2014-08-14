using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DataModel.BlogModel
{
    public class BlogRepository : IDisposable
    {
        private const string DATABASE_NAME = "SFBlogs";
        private const string COLLECTION_NAME = "SFBlogs";
        private static MongoClient client;
        private static MongoServer server;
        private static MongoDatabase database;


        public BlogRepository()
        {
            client = new MongoClient("mongodb://localhost");
            server = client.GetServer();
            database = server.GetDatabase(DATABASE_NAME);
        }

        public MongoCollection<Blog> GetCollection()
        {
            return database.GetCollection<Blog>(COLLECTION_NAME);
        }


        public void Dispose()
        {
            server.Disconnect();
        }
    }
}
