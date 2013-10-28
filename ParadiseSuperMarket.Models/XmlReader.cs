using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParadiseSuperMarket.Models
{
    public class XmlReader
    {
        const string XPathQuery = "/sales/sale";
        private XmlDocument XmlDoc;
        private XmlNodeList SaleList;
        private int SaleCount;
        private int ExpensesCount;

        public XmlReader(string path = "Vendor-Expenses.xml")
        {
            this.XmlDoc = new XmlDocument();
            this.XmlDoc.Load(path);
            this.SaleList = XmlDoc.SelectNodes(XPathQuery);
            this.SaleCount = -1;
            this.ExpensesCount = -1;
        }

        public bool ReadNextSale()
        {
            if (this.SaleCount + 1 < this.SaleList.Count)
            {
                this.SaleCount++;
                this.ExpensesCount = -1;
                return true;
            }
            
            return false;
        }

        public bool ReadNextExpenses()
        {
            if (this.ExpensesCount + 1 < this.SaleList[this.SaleCount].ChildNodes.Count)
            {
                this.ExpensesCount++;
                return true;
            }

            return false;
        }

        public string Month
        {
            get
            {
                return this.SaleList[this.SaleCount].ChildNodes[this.ExpensesCount].Attributes[0].Value;
            }
        }

        public string Vendor
        {
            get
            {
                return this.SaleList[this.SaleCount].Attributes[0].Value;
            }
        }

        public string Expenses
        {
            get
            {
                return this.SaleList[this.SaleCount].ChildNodes[this.ExpensesCount].ChildNodes[0].Value;
            }
        }

    }
}
