using System;

namespace LoginRegisterV2
{
    public partial class DashboardOverview : System.Web.UI.UserControl
    {
        private readonly DashboardService _dashboard = new DashboardService();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Loads the dashboard numbers and tables and shows them in this control.
        /// Called explicitly by the hosting page after its own role/session
        /// check has passed.
        /// </summary>
        public void LoadDashboard()
        {
            DashboardData data = _dashboard.Load();

            if (data.Overview != null)
            {
                lblTotalItems.Text = data.Overview.TotalItems.ToString();
                lblTotalUnits.Text = data.Overview.TotalUnits.ToString();
                lblLowStockCount.Text = data.Overview.LowStockCount.ToString();
                lblTotalValue.Text = "₱" + data.Overview.TotalValue.ToString("N2");
            }

            gvLowStock.DataSource = data.LowStockItems;
            gvLowStock.DataBind();

            gvRecentTransactions.DataSource = data.RecentTransactions;
            gvRecentTransactions.DataBind();

            gvRecentOrders.DataSource = data.RecentOrders;
            gvRecentOrders.DataBind();
        }
    }
}
