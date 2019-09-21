namespace Acme_Inventory_Entity
{
    public class SalesHistory
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public string StoreName { get; set; }
        public string Date { get; set; }
        public int Stock { get; set; }
        public int SalesQuantity { get; set; }

        public int StoreId { get; set; }
        
        public int ProductId { get; set; }

        public override string ToString()
        {
            return $"ID: {Id} | Store: {StoreName} | Product: {ProductName} | Date: {Date} | Stock: {Stock} | SalesQuantity:{SalesQuantity}";
        }
    }
}