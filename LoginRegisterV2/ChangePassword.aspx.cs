using System;

namespace LoginRegisterV2
{
    /// <summary>Change Password page: passes the form to PasswordChangeService and shows the result.</summary>
    public partial class ChangePassword : System.Web.UI.Page
    {
        private readonly PasswordChangeService _passwordChange = new PasswordChangeService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserSession.IsSignedIn)
            {
                Response.Redirect("Login.aspx");
            }
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            OperationResult result;
            try
            {
                result = _passwordChange.Change(UserSession.Email, txtNewPassword.Text, txtConfirmPassword.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error changing password: " + ex.Message;
                return;
            }

            if (!result.Success)
            {
                lblMessage.Text = result.Error;
                return;
            }

            UserSession.SignOut();
            Response.Redirect("Login.aspx");
        }
    }
}
