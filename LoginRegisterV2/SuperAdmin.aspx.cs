using System;

namespace LoginRegisterV2
{
    public partial class SuperAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserSession.IsInRole("Super Admin"))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            lblUserEmail.Text = UserSession.Email;
            dashboardOverview.LoadDashboard();
        }
    }
}
