namespace Acme_Inventory_Entity
{
    public class Store
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public double Profit { get; set; }

        public override string ToString()
        {
            return Profit > 0
                ? $"Id: {Id} | StoreName: {StoreName} | Profit: {Profit:C}"
                : $"Id: {Id} | StoreName: {StoreName}";
        }
    }
}