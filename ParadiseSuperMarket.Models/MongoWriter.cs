using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadiseSuperMarket.Models
{
    public class MongoWriter : Mongo
    {
        public MongoWriter( string collection ,string connectionString = "mongodb://localhost/",
            string database = "SupermarketDb")
            : base(connectionString,database,collection)
        {
        }

        public void WriteProduct(int productId, string productName, string vendorName,
            int totalQuantitySold, decimal totalIncomes, DateTime date)
        {
            BsonDocument newInput = new BsonDocument();
            newInput["ProductId"] = productId;
            newInput["ProductName"] = productName;
            newInput["VendorName"] = vendorName;
            newInput["TotalQuantitySold"] = totalQuantitySold;
            newInput["TotalIncomes"] = (float)totalIncomes;
            newInput["Date"] = date; //.ToString("dd-MMM-yyyy", base.Culture)
            base.Collection.Insert(newInput);
        }

        public void WriteExpenses(string vendor, string month, string expenses)
        {
            BsonDocument newInput = new BsonDocument();
            newInput["Vendor"] = vendor;
            newInput["Month"] = month;
            newInput["Expenses"] = expenses;
            base.Collection.Insert(newInput);
        }
    }
}
