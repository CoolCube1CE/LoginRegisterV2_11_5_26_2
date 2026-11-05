namespace LoginRegisterV2
{
    /// <summary>What an order or transaction needs to know about an item right now.</summary>
    public class StockLevel
    {
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
