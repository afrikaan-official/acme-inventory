using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using Acme_Inventory_Entity;


namespace Acme_Inventory_Data
{
    public class InventoryRepository
    {
        private readonly string _dbPath;
        public InventoryRepository()
        {
            _dbPath = Path.Combine(Directory.GetCurrentDirectory().Substring(0,
                Directory.GetCurrentDirectory().IndexOf("/bin/")), "db", "acme-inventory.db");
        }

        /// <summary>
        /// Gets all Sales Histories from sales table.
        /// </summary>
        /// <returns></returns>
        public List<SalesHistory> GetAllSales()
        {
            var result = new List<SalesHistory>();
            
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"select sl.Id,StoreName,ProductName,Date,SalesQuantity,Stock
                       from sales as sl inner join stores as st on sl.StoreId=st.Id 
                       inner join products as p on sl.ProductId=p.Id where SalesQuantity > 0;";

                var reader=cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new SalesHistory
                    {
                        Id =int.Parse(reader["Id"].ToString()),
                        StoreName = reader["StoreName"].ToString(),
                        Date = reader["Date"].ToString(),
                        Stock = int.Parse(reader["Stock"].ToString()),
                        ProductName = reader["ProductName"].ToString(),
                        SalesQuantity = int.Parse(reader["SalesQuantity"].ToString())
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Adds sales history to sales table
        /// </summary>
        /// <param name="history"></param>
        /// <returns></returns>
        public int AddSalesHistory(SalesHistory history)
        {
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                
                var cmd=connection.CreateCommand();
                cmd.CommandText = @"insert into sales  
                values (@ProductId,@StoreId,@Date,@SalesQuantity,@Stock)";
                
                cmd.Parameters.Add(new SQLiteParameter("@ProductId",history.ProductId.ToString()));
                cmd.Parameters.Add(new SQLiteParameter("@StoreId",history.StoreId.ToString()));
                cmd.Parameters.Add(new SQLiteParameter("@Date",history.Date));
                cmd.Parameters.Add(new SQLiteParameter("@SalesQuantity",history.SalesQuantity.ToString()));
                cmd.Parameters.Add(new SQLiteParameter("@Stock",history.Stock.ToString()));

                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets all stores from store table.
        /// </summary>
        /// <returns></returns>
        public List<Store> GetAllStores()
        {
            var result = new List<Store>();
         
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "select Id,StoreName from stores;";

                var reader=cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new Store
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        StoreName = reader["StoreName"].ToString(),
                        
                    });
                }

                return result;
            }
        }
        
        /// <summary>
        /// Gets all products from products table.
        /// </summary>
        /// <returns></returns>
        public List<Product> GetAllProducts()
        {
            var result = new List<Product>();
            
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "select Id,ProductName,Cost,SalesPrice from products;";

                var reader=cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new Product
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Cost = double.Parse(reader["Cost"].ToString()),
                        ProductName = reader["ProductName"].ToString(),
                        SalesPrice= double.Parse(reader["SalesPrice"].ToString())
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets available stock information for given store and product from sales table.
        /// </summary>
        /// <param name="storeId">StoreId</param>
        /// <param name="productId">ProductId</param>
        /// <returns></returns>
        public int GetStock(int storeId, int productId)
        {
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = @"select stock from sales where ProductId=@ProductId 
                            and StoreId=@StoreId order by Date desc limit 1;";

                cmd.Parameters.Add(new SQLiteParameter("@ProductId",productId));
                cmd.Parameters.Add(new SQLiteParameter("@StoreId",storeId));

                return int.Parse(cmd.ExecuteScalar().ToString());
            }
        }

        /// <summary>
        /// Deletes a record from sales
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int DeleteSalesHistory(int id)
        {
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                cmd.CommandText = "delete from sales where Id=@Id";
                cmd.Parameters.Add(new SQLiteParameter("@Id", id));

                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Selects sales record for given sales Id from sales table.
        /// </summary>
        /// <param name="salesId"></param>
        /// <returns></returns>
        public SalesHistory GetSales(int salesId)
        {
            var result = new SalesHistory();
            
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "select Id,StoreId,Date,Stock,ProductId,SalesQuantity from sales where Id=@Id;";
                cmd.Parameters.Add(new SQLiteParameter("@Id", salesId));
                
                var reader=cmd.ExecuteReader();

                while (reader.Read())
                {
                    result = new SalesHistory
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        StoreId = int.Parse(reader["StoreId"].ToString()),
                        Date = reader["Date"].ToString(),
                        Stock = int.Parse(reader["Stock"].ToString()),
                        ProductId = int.Parse(reader["ProductId"].ToString()),
                        SalesQuantity = int.Parse(reader["SalesQuantity"].ToString())
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// Updates  sales history record.
        /// </summary>
        /// <param name="sales"></param>
        /// <returns></returns>
        public int UpdateSalesHistory(SalesHistory sales)
        {
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                var cmd = connection.CreateCommand();
                cmd.CommandText = "update sales set SalesQuantity = @SalesQuantity, Stock=@Stock where Id=@Id";
                cmd.Parameters.Add(new SQLiteParameter("@SalesQuantity", sales.SalesQuantity));
                cmd.Parameters.Add(new SQLiteParameter("@Stock", sales.Stock));
                cmd.Parameters.Add(new SQLiteParameter("@Id", sales.Id));

               return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Calculates Profit based on:
        /// Sum of Sales * SalesPrice - Sum of Sales * Cost
        /// </summary>
        /// <param name="storeId"></param>
        /// <returns>profit</returns>
        public int GetProfitByStore(int storeId)
        {
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select Sum(Profit) from 
                (select Sum(SalesQuantity) * SalesPrice - Sum(SalesQuantity) * Cost as Profit
                from sales as sl inner join stores as st on sl.StoreId=st.Id 
                inner join products as p on sl.ProductId=p.Id where SalesQuantity > 0 
                and StoreId=@StoreId group by ProductId);";

                cmd.Parameters.Add(new SQLiteParameter("@StoreId", storeId.ToString()));

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        /// <summary>
        /// Gets all stores order by profits
        /// </summary>
        /// <returns>List<Store></returns>
        public List<Store> GetStoresOrdered()
        {
            var result = new List<Store>();
            using (var connection = new SQLiteConnection($"Data Source = {_dbPath}; Version=3;", true))
            {
                connection.Open();
                var cmd = connection.CreateCommand();

                cmd.CommandText = @"select StoreId,StoreName,Sum(Profit)As Profit from 
                (select StoreId,StoreName,ProductName, Sum(SalesQuantity), Sum(SalesQuantity) * SalesPrice - Sum(SalesQuantity) * Cost as Profit
                from sales as sl inner join stores as st on sl.StoreId=st.Id 
                inner join products as p on sl.ProductId=p.Id where SalesQuantity > 0  group by ProductId,StoreId) group by StoreName order by Profit desc;";

                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    result.Add(new Store
                    {
                        Id = int.Parse(reader["StoreId"].ToString()),
                        Profit = double.Parse(reader["Profit"].ToString()),
                        StoreName = reader["StoreName"].ToString()
                    });
                }

                return result;
            }
        }
    }
}