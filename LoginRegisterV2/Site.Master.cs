using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LoginRegisterV2
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            bool isLoggedIn = UserSession.IsSignedIn;

            navBar.Visible = isLoggedIn;
            profileBar.Visible = isLoggedIn;

            if (!isLoggedIn)
            {
                return;
            }

            string accessType = UserSession.Role ?? "User";

            lblProfileEmail.Text = UserSession.Email;
            lblProfileRole.Text = accessType;

            switch (accessType)
            {
                case "Super Admin":
                    lnkNavDashboard.NavigateUrl = "~/SuperAdmin.aspx";
                    break;
                case "Admin":
                    lnkNavDashboard.NavigateUrl = "~/AdminPage.aspx";
                    break;
                default:
                    lnkNavDashboard.NavigateUrl = "~/UserHome.aspx";
                    break;
            }

            HighlightActiveLink();
        }

        private void HighlightActiveLink()
        {
            string currentPage = Path.GetFileName(Request.Path);

            MarkActive(lnkNavDashboard, currentPage, "UserHome.aspx", "AdminPage.aspx", "SuperAdmin.aspx");
            MarkActive(lnkNavInventory, currentPage, "Inventory.aspx");
            MarkActive(lnkNavStockTransactions, currentPage, "StockTransactions.aspx");
            MarkActive(lnkNavOrders, currentPage, "Orders.aspx");
        }

        private void MarkActive(HyperLink link, string currentPage, params string[] matches)
        {
            bool isActive = matches.Any(match => string.Equals(currentPage, match, StringComparison.OrdinalIgnoreCase));
            link.CssClass = isActive ? "app-nav__link is-active" : "app-nav__link";
        }

        protected void lnkChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ChangePasswordOTP.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            UserSession.SignOut();
            Response.Redirect("~/Login.aspx");
        }
    }
}
