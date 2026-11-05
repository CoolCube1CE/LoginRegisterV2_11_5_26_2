namespace LoginRegisterV2
{
    /// <summary>Collects everything the dashboard shows. It decides what and how many; the repositories fetch it.</summary>
    public class DashboardService
    {
        private const int LowStockRows = 5;
        private const int RecentRows = 6;

        private readonly StockItemRepository _items = new StockItemRepository();
        private readonly StockTransactionRepository _transactions = new StockTransactionRepository();
        private readonly OrderRepository _orders = new OrderRepository();

        public DashboardData Load()
        {
            return new DashboardData
            {
                Overview = _items.GetOverview(),
                LowStockItems = _items.GetLowStock(LowStockRows),
                RecentTransactions = _transactions.GetRecent(RecentRows),
                RecentOrders = _orders.GetRecent(RecentRows)
            };
        }
    }
}
