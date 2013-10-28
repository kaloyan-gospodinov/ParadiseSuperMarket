using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Globalization;
using System.Collections;

namespace ParadiseSuperMarket.Models
{
    public class Mongo
    {
        protected MongoClient Client;
        protected MongoDatabase Database;
        protected MongoCollection Collection;
        protected MongoServer Server;
        protected CultureInfo Culture;

        public Mongo(string connectionString, string database, string collection)
        {
            this.Client = new MongoClient(connectionString);
            this.Server = this.Client.GetServer();
            this.Database = this.Server.GetDatabase(database);
            this.Collection = this.Database.GetCollection(collection);
            this.Culture = new CultureInfo("en-US", false); 
        }
    }
}
