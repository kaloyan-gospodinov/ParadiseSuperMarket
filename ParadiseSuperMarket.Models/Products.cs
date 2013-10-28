using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace ParadiseSuperMarket.Models
{
    public class Products
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string VendorName { get; set; }
        public int TotalQuantitySold { get; set; }
        public int TotalIncomes { get; set; }
        public DateTime Date { get; set; }

        [BsonConstructor]
        public Products(int productId, string productName, string vendorName,
            int totalQuantitySold, int totalIncomes, DateTime date)
        {
            this.ProductId = productId;
            this.ProductName = productName;
            this.VendorName = vendorName;
            this.TotalQuantitySold = totalQuantitySold;
            this.TotalIncomes = totalIncomes;
            this.Date = date;
        }
    }
}
