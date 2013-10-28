using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ParadiseSuperMarket.Models
{
    public static class JSON
    {
        const string ReportPath = @"../../../../Product-Reports/";
        public static void Extract(int productId, string productName, 
            string vendorName, int totalQuantitySold, decimal totalIncomes)
        {
            string filePath = ReportPath + productId + ".json";

            StreamWriter file = new StreamWriter(filePath);
            using (file)
            {
                string jsonObject = @"{" +  Environment.NewLine + 
                                         "'product-id' : " + productId + "," +  Environment.NewLine +
                                         "'product-name' : '" + productName + "'," + Environment.NewLine +
                                         "'vendor-name' : '" + vendorName + "'," + Environment.NewLine +
                                         "'totalQuantitySold' : " + totalQuantitySold + "," + Environment.NewLine + 
                                         "'total-quantity-sold' : " + totalIncomes + "," + 
                                         Environment.NewLine + "}";
                file.Write(jsonObject);
            }
        }
    }
}
