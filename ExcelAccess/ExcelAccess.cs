using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ExcelAccess
{
    public delegate void ExcelData(List<object> rowData);
    private OleDbConnection oleDbConnection;
    private OleDbConnectionStringBuilder oleDbConnectionSb;
    private string filePath;

    public ExcelAccess(string filePath)
    {
        this.filePath = filePath;
        this.oleDbConnectionSb = new OleDbConnectionStringBuilder();
        this.oleDbConnectionSb.Provider = "Microsoft.ACE.OLEDB.12.0";
        this.oleDbConnectionSb.DataSource = this.filePath;
        this.oleDbConnectionSb.Add("Extended Properties", "Excel 12.0 Xml;HDR=YES;IMEX=1");
    }

    public void Open()
    {
        this.oleDbConnection =
            new OleDbConnection(this.oleDbConnectionSb.ConnectionString);
        oleDbConnection.Open();
    }

    public void Close()
    {
        this.oleDbConnection.Close();
    }

    public void ReadSheetActionRow(string sheetName, ExcelData excelRowAction)
    {
        DataSet sheet1 = new DataSet();
        string selectSql = @"SELECT * FROM [" + sheetName + "$]";
        using (OleDbDataAdapter adapter =
            new OleDbDataAdapter(selectSql, this.oleDbConnection))
        {
            adapter.Fill(sheet1);
        }

        var table = sheet1.Tables[0];
        foreach (DataRow row in table.Rows)
        {
            List<object> currentRow = new List<object>(table.Columns.Count);
            foreach (DataColumn column in table.Columns)
            {
                if (row[column] + "" != "")
                {
                    currentRow.Add(row[column]);
                }
            }
            excelRowAction(currentRow);
        }
    }
}
