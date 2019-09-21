namespace Acme_Inventory_Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double Cost { get; set; }
        public double SalesPrice { get; set; }


        public override string ToString()
        {
            return $"Id: {Id} | ProductName: {ProductName} | Cost {Cost:C} | SalesPrice: {SalesPrice:C}";
        }
    }
}