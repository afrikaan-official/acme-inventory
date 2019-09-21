using System;
using Acme_Inventory_Data;
using Acme_Inventory_Entity;

namespace Acme_Inventory
{
    class Program
    {
        private static InventoryRepository _repository=new InventoryRepository();

        //main part of program
        static void Main(string[] args)
        {
            var operation = 0;
            do
            {
                Console.WriteLine(@"Choose an operation to proceed:
1- Get sales history
2- Add new sales history record
3- Delete sales history record
4- Update sales history record
5- Get profit for given store
6- Get the most profitable store
99- Exit");
                try
                {
                    operation = Convert.ToInt16(Console.ReadLine());
                    
                    switch (operation)
                    {
                        case 1:
                        {
                            Console.WriteLine("Fetching all sales histories...");
                            _repository.GetAllSales().ForEach(x => { Console.WriteLine(x.ToString()); });
                        }
                            break;
                        case 2:
                        {
                            AddSalesHistory();
                        }
                            break;
                        case 3:
                        {
                            Console.WriteLine("Choose a record to delete!Type Id to select:\n");

                            _repository.GetAllSales().ForEach(x=>Console.WriteLine(x.ToString()));

                            var salesId = Convert.ToInt32(Console.ReadLine()); //assume they will enter proper value

                            Console.WriteLine(_repository.DeleteSalesHistory(salesId) >= 1
                                ? "Record deleted."
                                : "Could not delete sales history! You can try again");
                        }
                            break;
                        case 4:
                        {
                            Console.Write("You can edit only sales quantity value!Type Id to select record:\n");

                            _repository.GetAllSales().ForEach(x => Console.WriteLine(x.ToString()));

                            var salesId = Convert.ToInt32(Console.ReadLine()); //assume they will enter proper value

                            var sales = _repository.GetSales(salesId);

                            Console.WriteLine("Insert new sales quantity value:");

                            var newQuantity = Convert.ToInt32(Console.ReadLine()); //assume they will enter proper value
                            
                            //calculate stock again by adding old stock value with old sales quantity
                            var adjustedStock =
                                sales.Stock +
                                sales.SalesQuantity; 

                            sales.SalesQuantity = newQuantity;
                            sales.Stock = adjustedStock - newQuantity;

                            Console.WriteLine(_repository.UpdateSalesHistory(sales) >= 1
                                ? "Sales Record has been Updated."
                                : "Could not add sales history.");
                        }
                            break;
                        case 5:
                        {
                            Console.WriteLine("Choose a Store to continue");
                            _repository.GetAllStores().ForEach(x => Console.WriteLine(x.ToString()));
                            var storeId = Convert.ToInt32(Console.ReadLine()); // assume they will enter proper value
                            
                            Console.WriteLine(_repository.GetProfitByStore(storeId).ToString("C"));
                        }
                            break;
                        case 6:
                        {
                            _repository.GetStoresOrdered().ForEach(x => Console.WriteLine(x.ToString()));
                        }
                            break;

                        case 99:
                        {
                            Console.WriteLine("Exiting the app!");
                            Environment.Exit(-1);
                        }
                            break;

                        default:
                            Console.WriteLine("Wrong input! Please select an option from the list!");
                            break;
                    }
                }
                catch (Exception)
                {
                    //Do nothing. Just warn user and continue.
                    Console.WriteLine("Wrong input!");
                }
                finally
                {
                    //to continue set operation to 0.
                    operation = 0;
                }
                
            } while (operation == 0);
        }

        /// <summary>
        /// Adds sales history record.
        /// Adding is long process so it expects from user to choose first store and product'
        /// </summary>
        private static void AddSalesHistory()
        {
            Console.WriteLine("Choose a Store to continue");
            _repository.GetAllStores().ForEach(x => Console.WriteLine(x.ToString()));
            var storeId = Convert.ToInt32(Console.ReadLine()); // assume they will enter proper value

            Console.WriteLine("Choose a Product to continue:");
            _repository.GetAllProducts().ForEach(x => Console.WriteLine(x.ToString()));
            var productId = Convert.ToInt32(Console.ReadLine()); // assume they will enter proper value

            var stock = _repository.GetStock(storeId, productId);
            Console.WriteLine($"Current Stock is {stock}. Please enter sales quantity:");
            var salesQuantity = Convert.ToInt32(Console.ReadLine()); // assume they will enter proper value

            if (salesQuantity != 0)
            {
                var salesHistory = new SalesHistory
                {
                    Date = DateTime.Now.ToString("yyyy-MM-dd"), //assume that cant be changed
                    Stock = stock - salesQuantity,
                    ProductId = productId,
                    StoreId = storeId,
                    SalesQuantity = salesQuantity
                };

                Console.WriteLine(_repository.AddSalesHistory(salesHistory) >= 1
                    ? "Sales Record has been added."
                    : "Could not add sales history.'");    
            }
            else
            {
                Console.WriteLine("Please enter valid number for Sales Quantity");
            }
        }
        
    }
}

