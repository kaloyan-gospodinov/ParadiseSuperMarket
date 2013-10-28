using System;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using EntityFramework.Data;
using System.Linq;

namespace ParadiseSuperMarket.Models
{
    public class PDFGenerator
    {
        private const float StandardSize = 12f;
        private const string ValuesFormattingString = "{0:0.00}";
        private const string DatesFormattingString = "{0:dd.MM.yyyy}";
        private static readonly Font defaultBoldFont = new Font(Font.FontFamily.TIMES_ROMAN, StandardSize, Font.BOLD);
        private static readonly GrayColor backgroundColor = new GrayColor(215);

        public static void GeneratePDF()
        {
            Document document = new Document();
            string path = @"D:\test\report.pdf";
            float[] widths = new float[] { 5f, 3f, 3f, 8f, 2f };
            decimal grandTotal = 0;
            try
            {
                PdfWriter.GetInstance(document, new FileStream(path, FileMode.Create));
                document.Open();
                PdfPTable table = new PdfPTable(5);
                table.SetWidths(widths);
                
                CreteHeaderLine(table);
                IQueryable<EntityFramework.Data.Dates> dates = DatabaseToPDFDataMapper.ReadDates();
                foreach (EntityFramework.Data.Dates date in dates)
                {
                    CreateSubtableHeader(date, table);
                    IQueryable<EntityFramework.Data.Sales> sales = DatabaseToPDFDataMapper.ReadProductsByDate(date.Id);
                    foreach (EntityFramework.Data.Sales sale in sales)
                    {
                        CreateTableRow(sale, table);
                    }

                    CreateDelimitingRow(table);
                    decimal totalSum = CalculateSum(sales);
                    CreateTotalSumRow(table, totalSum, date);
                    grandTotal += totalSum;
                }

                CreateFooterLine(table, grandTotal);
                document.Add(table);
            }
            finally
            {
                document.Close();
                System.Diagnostics.Process.Start(path);
            }
        }

        private static void CreateTableRow(EntityFramework.Data.Sales sale, PdfPTable table)
        {
            table.AddCell(sale.Products.Name);

            PdfPCell quantity = new PdfPCell(new Phrase(sale.Quantity.ToString() + " " + sale.Products.Measurements.Name));
            quantity.HorizontalAlignment = 1;
            table.AddCell(quantity);

            PdfPCell unitPrice = new PdfPCell(new Phrase(String.Format(ValuesFormattingString, sale.UnitPrice)));
            unitPrice.HorizontalAlignment = 1;
            table.AddCell(unitPrice);

            table.AddCell(sale.Supermarkets.Name);
            
            PdfPCell sum = new PdfPCell(new Phrase(String.Format(ValuesFormattingString, sale.Sum)));
            sum.HorizontalAlignment = 2;
            table.AddCell(sum);
        }
  
        private static void CreateFooterLine(PdfPTable table, decimal grandTotal)
        {
            PdfPCell totalSumPhrase = new PdfPCell(new Phrase("Grand total:"));
            totalSumPhrase.Colspan = 4;
            totalSumPhrase.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
            table.AddCell(totalSumPhrase);

            PdfPCell grandTotalSumValue = new PdfPCell(new Phrase(String.Format(ValuesFormattingString, grandTotal), new Font(Font.FontFamily.TIMES_ROMAN, StandardSize, Font.BOLD)));
            grandTotalSumValue.HorizontalAlignment = 2;
            table.AddCell(grandTotalSumValue);
        }
  
        private static void CreateDelimitingRow(PdfPTable table)
        {
            for (int i = 0; i < 5; i++)
            {
                PdfPCell cell = new PdfPCell(new Phrase("..."));
                cell.HorizontalAlignment = 1;
                table.AddCell(cell);
            }
        }

        private static void CreateTotalSumRow(PdfPTable table, decimal totalSum, EntityFramework.Data.Dates date)
        {
            PdfPCell totalSumPhrase = new PdfPCell(new Phrase("Total Sum for " + String.Format(DatesFormattingString, date.Date) + ":"));
            totalSumPhrase.Colspan = 4;
            totalSumPhrase.HorizontalAlignment = 2; //0=Left, 1=Centre, 2=Right
            table.AddCell(totalSumPhrase);

            PdfPCell totalSumValue = new PdfPCell(new Phrase(String.Format(ValuesFormattingString, totalSum), defaultBoldFont));
            totalSumValue.HorizontalAlignment = 2;
            table.AddCell(totalSumValue);
        }
  
        private static void CreteHeaderLine(PdfPTable table)
        {
            PdfPCell header = new PdfPCell(new Phrase("Aggregated Sales Report", defaultBoldFont));
            header.Colspan = 5;
            header.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
            table.AddCell(header);
        }
  
        private static void CreateSubtableHeader(EntityFramework.Data.Dates date, PdfPTable table)
        {
            PdfPCell dateOfSubtable = new PdfPCell(new Phrase(String.Format(DatesFormattingString, date.Date)));
            dateOfSubtable.Colspan = 5;
            dateOfSubtable.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            dateOfSubtable.BackgroundColor = backgroundColor;
            table.AddCell(dateOfSubtable);

            PdfPCell product = new PdfPCell(new Phrase("Product", defaultBoldFont));
            product.BackgroundColor = backgroundColor;
            table.AddCell(product);

            PdfPCell quantity = new PdfPCell(new Phrase("Quantity", defaultBoldFont));
            quantity.BackgroundColor = backgroundColor;
            table.AddCell(quantity);

            PdfPCell unitPrice = new PdfPCell(new Phrase("Unit Price", defaultBoldFont));
            unitPrice.BackgroundColor = backgroundColor;
            table.AddCell(unitPrice);

            PdfPCell location = new PdfPCell(new Phrase("Location", defaultBoldFont));
            location.BackgroundColor = backgroundColor;
            table.AddCell(location);

            PdfPCell sum = new PdfPCell(new Phrase("Sum", defaultBoldFont));
            sum.BackgroundColor = backgroundColor;
            table.AddCell(sum);
        }

        private static decimal CalculateSum(IQueryable<EntityFramework.Data.Sales> sales)
        {
            decimal result = 0;
            foreach (EntityFramework.Data.Sales sale in sales)
            {
                result += sale.Sum;
            }

            return result;
        }
    }
}