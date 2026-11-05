namespace LoginRegisterV2
{
    /// <summary>One row to insert into StockTransactions.</summary>
    public class StockTransactionEntry
    {
        public int ItemId { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string Reference { get; set; }
        public string PerformedBy { get; set; }
        public string Notes { get; set; }
    }
}
