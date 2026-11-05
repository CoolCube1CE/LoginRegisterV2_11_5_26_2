using System;

namespace LoginRegisterV2
{
    public partial class UserHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserSession.IsInRole("User"))
            {
                Response.Redirect("Login.aspx");
                return;
            }

            lblUserEmail.Text = UserSession.Email;
            dashboardOverview.LoadDashboard();
        }
    }
}
