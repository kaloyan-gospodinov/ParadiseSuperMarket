using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ParadiseSuperMarket.Models
{
    public class XMLWriter
    {
        const string ReportPath = @"../../../../Product-Reports/Sales-by-Vendors-report.xml";
        private XElement XmlRootElement;
        private XElement SubRootElement;
        private CultureInfo Culture;

        public XMLWriter(string xmlRootElemt = "sales")
        {
            this.XmlRootElement = new XElement(xmlRootElemt);
            Culture = new CultureInfo("en-US", false); 
        }

        public void AddNewElement( string vendor, string SubRootElementName = "sale")
        {
            XAttribute vendorAtt = new XAttribute("vendor", vendor);
            this.SubRootElement = new XElement(SubRootElementName,vendorAtt);
        }

        public void AddNewSubElement(DateTime date, decimal totalSum)
        {
            XAttribute dateAttribute = new XAttribute("date", date.ToString("dd-MMM-yyyy", Culture));
            XAttribute sumAttribute = new XAttribute("total-sum", totalSum);
            XElement newElement = new XElement("summary",dateAttribute,sumAttribute);
            this.SubRootElement.Add(newElement);
        }

        public void EndElement()
        {
            this.XmlRootElement.Add(this.SubRootElement);
            this.XmlRootElement.Save(ReportPath);
        }
    }
}
