using System;

namespace LoginRegisterV2
{
    /// <summary>Login page: reads the two fields, asks AccountService, then signs the user in and redirects.</summary>
    public partial class Login : System.Web.UI.Page
    {
        private readonly AccountService _accounts = new AccountService();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (email == "" || password == "")
            {
                lblMessage.Text = "Please fill in all the fields";
                return;
            }

            UserCredentials user;
            try
            {
                user = _accounts.Authenticate(email, password);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Connection Error: " + ex.Message;
                return;
            }

            if (user == null)
            {
                lblMessage.Text = "Incorrect Username/Password";
                return;
            }

            UserSession.SignIn(email, user.AccessType);
            Response.Redirect(HomePageFor(user.AccessType));
        }

        protected void lnkRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }

        private static string HomePageFor(string accessType)
        {
            if (accessType == "Super Admin") return "SuperAdmin.aspx";
            if (accessType == "Admin") return "AdminPage.aspx";
            return "UserHome.aspx";
        }
    }
}
