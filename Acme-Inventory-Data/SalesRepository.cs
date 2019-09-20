using System;
using System.Data.SQLite;
using System.IO;


namespace Acme_Inventory_Data
{
    public class SalesRepository
    {
        public SalesRepository()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "db", "acme-inventory.db");
            var bag = new SQLiteConnection($"Data Source = {path}; Version=3;", true);

            bag.Open();

            Console.WriteLine(bag.State);
        }
    }
}