namespace LoginRegisterV2
{
    /// <summary>The four headline numbers on the dashboard.</summary>
    public class StockOverview
    {
        public int TotalItems { get; set; }
        public int TotalUnits { get; set; }
        public int LowStockCount { get; set; }
        public decimal TotalValue { get; set; }
    }
}
