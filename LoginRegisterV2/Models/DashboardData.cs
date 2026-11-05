using System.Data;

namespace LoginRegisterV2
{
    /// <summary>Everything the dashboard control shows, gathered in one object.</summary>
    public class DashboardData
    {
        /// <summary>Null when the overview query returned no row.</summary>
        public StockOverview Overview { get; set; }

        public DataTable LowStockItems { get; set; }
        public DataTable RecentTransactions { get; set; }
        public DataTable RecentOrders { get; set; }
    }
}
