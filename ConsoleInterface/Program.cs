using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using ParadiseSuperMarket.Models;
using EntityFramework.Data;
using OpenAccess.Data;
using SQLite;
using System.Threading;

namespace ConsoleInterface
{
    public class Program
    {
        static void ExcelReports()
        {
            //Console.WriteLine("Input zip file path: ");
            //string zipFilePath = Console.ReadLine();
            ArchiveAccess archive = new ArchiveAccess(@"E:\newTeamwork\project\Sample-Reports.zip");
            archive.Extract();

            OpenAccessMySQL mysqlCon = new OpenAccessMySQL();
            ParadiseSupermarketChainEntities sqlCon = new ParadiseSupermarketChainEntities();

            using (mysqlCon)
            {
                using (sqlCon)
                {
                    foreach (var product in mysqlCon.Products)
                    {
                        var efProduct = new EntityFramework.Data.Products();
                        efProduct.Id = product.Id;
                        efProduct.Name = product.ProductName;
                        efProduct.BasePrice = (decimal)product.BasePrice;

                        var measure = sqlCon.Measurements.Where(m => m.Name == product.Measure.MeasureName).FirstOrDefault();
                        if (measure == null)
                        {
                            measure = new EntityFramework.Data.Measurements();
                            measure.Name = product.Measure.MeasureName;
                            sqlCon.Measurements.Add(measure);
                        }

                        efProduct.Measurements = measure;

                        var vendor = sqlCon.Vendors.Where(v => v.Name == product.Vendor.VendorName).FirstOrDefault();
                        if (vendor == null)
                        {
                            vendor = new EntityFramework.Data.Vendors();
                            vendor.Name = product.Vendor.VendorName;
                            sqlCon.Vendors.Add(vendor);
                        }

                        efProduct.Vendors = vendor;

                        sqlCon.Products.Add(efProduct);
                        try
                        {
                            sqlCon.SaveChanges();
                        }
                        catch (Exception e)
                        {
                            //Console.WriteLine(e.InnerException.InnerException.Message);
                        }
                    }
                }
            }

            // get the info from the unzipped folder
            string unzippedFolderName = "Paradise-Sample-Reports";
            string unzippedFolderPath = String.Format(@"../../../{0}", unzippedFolderName);

            foreach (var dir in Directory.GetDirectories(archive.ExtractPath))
            {
                foreach (var file in Directory.GetFiles(dir, "*.xls"))
                {
                    string currentFolderName = Path.GetFileName(dir);
                    DateTime currentDate = DateTime.Parse(currentFolderName);
                    string fileName = Path.GetFileName(file);

                    using (var db = new ParadiseSupermarketChainEntities())
                    {
                        var excelComs = new ExcelAccess(file);
                        string supermarketName = null;
                        int rowIndex = 0;

                        excelComs.Open();

                        excelComs.ReadSheetActionRow("Sales", (row) =>
                        {
                            rowIndex++;
                            if (rowIndex <= 2)
                            {
                                if (row.Count == 1 && row[0].ToString().IndexOf("Supermarket") != -1)
                                {
                                    // this is the supermarket Name
                                    supermarketName = row[0] + "";
                                }
                                // skip the first 2 rows
                                return;
                            }

                            if (row.Count == 4)
                            {
                                // add a product
                                int productId = int.Parse((row[0] + ""));
                                double quantity = double.Parse((row[1] + ""));
                                decimal unitPrice = decimal.Parse((row[2] + ""));
                                decimal sum = decimal.Parse((row[3] + ""));

                                Sales productSales = new Sales();
                                productSales.ProductId = productId;
                                productSales.Quantity = quantity;
                                productSales.UnitPrice = unitPrice;
                                productSales.Sum = sum;

                                // find out if the supermarket exists

                                Supermarkets supermarket =
                                        db.Supermarkets.Where(s =>
                                            s.Name == supermarketName).FirstOrDefault();
                                if (supermarket == null)
                                {
                                    supermarket = new Supermarkets();
                                    supermarket.Name = supermarketName;
                                    db.Supermarkets.Add(supermarket);
                                }

                                productSales.Supermarkets = supermarket;

                                // find out if the date exists
                                Dates date =
                                    db.Dates.Where(d => d.Date == currentDate).FirstOrDefault();
                                if (date == null)
                                {
                                    date = new Dates();
                                    date.Date = currentDate;
                                    db.Dates.Add(date);
                                }

                                productSales.Dates = date;

                                db.Sales.Add(productSales);
                                db.SaveChanges();
                            }
                            else
                            {
                                // the final line: the sum of the products
                            }
                        });
                        excelComs.Close();
                    }
                }
            }
            Directory.Delete(archive.ExtractPath, true);
        }

        static void VendorXml()
        {
            ParadiseSupermarketChainEntities sqlCon = new ParadiseSupermarketChainEntities();

            var query =
                from product in sqlCon.Products
                join vendor in sqlCon.Vendors on
                product.VendorId equals vendor.Id
                join sale in sqlCon.Sales on
                product.Id equals sale.ProductId
                join date in sqlCon.Dates on
                sale.DateId equals date.Id
                orderby date.Date
                orderby product.Name
                select new
                {
                    Price = product.BasePrice,
                    Name = vendor.Name,
                    Date = date.Date
                };

            Dictionary<Tuple<string, DateTime>, decimal> vendorCollection = new Dictionary<Tuple<string, DateTime>, decimal>();

            foreach (var item in query)
            {
                if (!vendorCollection.ContainsKey(new Tuple<string, DateTime>(item.Name, item.Date)))
                {
                    vendorCollection.Add(new Tuple<string, DateTime>(item.Name, item.Date), 0);

                }

                vendorCollection[new Tuple<string, DateTime>(item.Name, item.Date)] += item.Price;
            }

            XMLWriter writer = new XMLWriter();
            string vendorName = "";

            foreach (var item in vendorCollection)
            {
                if (vendorName == item.Key.Item1)
                {
                    writer.AddNewSubElement(item.Key.Item2, item.Value);
                }
                else
                {
                    writer.AddNewElement(item.Key.Item1);
                    writer.AddNewSubElement(item.Key.Item2, item.Value);
                    writer.EndElement();
                    vendorName = item.Key.Item1;
                }
            }
        }
        //PDFGenerator.GeneratePDF();

        static void ProductReportsToJsonAndMongo()
        {
            ParadiseSupermarketChainEntities sqlCon = new ParadiseSupermarketChainEntities();
            var query =
                from product in sqlCon.Products
                join vendor in sqlCon.Vendors on
                product.VendorId equals vendor.Id
                join sale in sqlCon.Sales on
                product.Id equals sale.ProductId
                join date in sqlCon.Dates on
                sale.DateId equals date.Id
                orderby product.Id
                select new
                {
                    Id = product.Id,
                    Name = product.Name,
                    Quantity = sale.Quantity,
                    Sum = sale.Sum,
                    VendorName = vendor.Name,
                    Date = date.Date
                };

            Dictionary<int, Tuple<string, int, decimal, string, DateTime>> collection = new Dictionary<int, Tuple<string, int, decimal, string, DateTime>>();

            foreach (var item in query)
            {
                if (!collection.ContainsKey(item.Id))
                {
                    collection.Add(item.Id, new Tuple<string, int, decimal, string, DateTime>((string)item.Name, (int)item.Quantity, (decimal)item.Sum, (string)item.VendorName, (DateTime)item.Date));
                    continue;
                }

                int newQuantity = collection[item.Id].Item2 + (int)item.Quantity;
                decimal newSum = collection[item.Id].Item3 + item.Sum;

                collection[item.Id] = new Tuple<string, int, decimal, string, DateTime>((string)item.Name, newQuantity, newSum, (string)item.VendorName, (DateTime)item.Date);
            }

            foreach (var item in collection)
            {
                JSON.Extract(item.Key, item.Value.Item1, item.Value.Item4, item.Value.Item2, item.Value.Item3);
                MongoWriter mongoWriter = new MongoWriter("Products");
                mongoWriter.WriteProduct(item.Key, item.Value.Item1, item.Value.Item4, item.Value.Item2, item.Value.Item3, item.Value.Item5);
            }
        }

        static void LoadVendor()
        {
            ParadiseSupermarketChainEntities sqlCon = new ParadiseSupermarketChainEntities();
            using (sqlCon)
            {
                MongoWriter mongo = new MongoWriter("Expenses");
                XmlReader reader = new XmlReader("E:\\Vendor-Expenses.xml");
                while (reader.ReadNextSale())
                {
                    while (reader.ReadNextExpenses())
                    {
                        mongo.WriteExpenses(reader.Vendor, reader.Month, reader.Expenses);
                        var expence = new EntityFramework.Data.Expenses()
                        {
                            Expenses1 = decimal.Parse(reader.Expenses),
                            ExpensesDate = DateTime.Parse(reader.Month)
                        };
                        int vendorIdd = 1;
                        var VendorId =
                           from vendor in sqlCon.Vendors
                           where vendor.Name == reader.Vendor
                           select new
                              {
                                  Id = vendor.Id
                              };
                        var vendorIdc = sqlCon.Vendors.Where(v => v.Name == reader.Vendor).FirstOrDefault();

                        expence.VendorId = vendorIdd;

                        sqlCon.Expenses.Add(expence);
                        sqlCon.SaveChanges();
                    }
                }
            }
        }

        static int Menu()
        {
            Console.WriteLine("Input: " + Environment.NewLine +
                "1) Load Excel Reports from ZIP File" + Environment.NewLine +
                "2) Generate PDF Aggregated Sales Reports" + Environment.NewLine +
                "3) Generate XML Sales Report by Vendors" + Environment.NewLine +
                "4) Product Reports" + Environment.NewLine +
                "5) Load Vendor Expenses from XML" + Environment.NewLine +
                "6) Vendors Total Report" + Environment.NewLine +
                "7) Exit");

            int input = Convert.ToInt32(Console.ReadLine());
            return input;
        }

        static void VendorsTotalReport()
        {
            MongoReader test = new MongoReader("Products");
            test.Read();

            foreach (var item in test)
            {
                Console.WriteLine(item);
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                int input = Menu();
                switch (input)
                {
                    case 1: ExcelReports(); break;
                    case 2: PDFGenerator.GeneratePDF(); break;
                    case 3: VendorXml(); break;
                    case 4: ProductReportsToJsonAndMongo(); break;
                    case 5: LoadVendor(); break;
                    case 6: VendorsTotalReport(); break;
                    default: return;
                }
            }
        }
    }
}
