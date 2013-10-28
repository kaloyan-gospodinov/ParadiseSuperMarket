using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Collections;

namespace ParadiseSuperMarket.Models
{
    public class MongoReader : Mongo, IEnumerable
    {
        public MongoDB.Driver.MongoCursor<ParadiseSuperMarket.Models.Products> collection;

        public MongoReader( string collection, string connectionString = "mongodb://localhost/",
            string database = "SupermarketDb")
            : base(connectionString,database,collection)
        {
        }

        public void Read()
        {
            collection = base.Collection.FindAllAs<Products>();
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var item in collection)
            {
                yield return item;
            }
        }
    }
}
