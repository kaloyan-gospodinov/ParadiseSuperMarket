using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace ParadiseSuperMarket.Models
{
    public static class SqlLite
    {
        public static void Write(string productName, int tax)
        {
            string connectionString = @"Data Source=../../../../Product-Reports/Vendors.db;Version=3;";
            SQLiteConnection dbcon = new SQLiteConnection(connectionString);
            dbcon.Open();

            string query = "INSERT INTO Vendors(ProductName,Tax)" +
                "Values(@productName,@tax)";
            SQLiteCommand command = new SQLiteCommand(query, dbcon);
            command.Parameters.AddWithValue("@productName", productName);
            command.Parameters.AddWithValue("@tax", tax);

            command.ExecuteNonQuery();
        }

        public static void Read()
        {
            string connectionString = @"Data Source=../../../../Product-Reports/Vendors.db;Version=3;";
            SQLiteConnection dbcon = new SQLiteConnection(connectionString);
            dbcon.Open();

            string query = "SELECT * FROM Vendors";
            SQLiteCommand command = new SQLiteCommand(query, dbcon);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Console.WriteLine(reader["ProductName"] + " " + reader["Tax"]);
            }

        }
    }
}
