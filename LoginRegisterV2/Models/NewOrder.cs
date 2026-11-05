namespace LoginRegisterV2
{
    /// <summary>One row to insert into Orders (it always starts as Pending).</summary>
    public class NewOrder
    {
        public string CustomerName { get; set; }
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string PlacedBy { get; set; }
    }
}
