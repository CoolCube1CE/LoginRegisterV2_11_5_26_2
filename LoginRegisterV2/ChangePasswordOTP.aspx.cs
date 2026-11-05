using System;

namespace LoginRegisterV2
{
    /// <summary>Change Password OTP page: asks PasswordChangeService to e-mail a code, and checks what the user types.</summary>
    public partial class ChangePasswordOTP : System.Web.UI.Page
    {
        private const string OtpSessionKey = "ChPW_OTP";

        private readonly PasswordChangeService _passwordChange = new PasswordChangeService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!UserSession.IsSignedIn)
            {
                Response.Redirect("Login.aspx");
                return;
            }

            if (!IsPostBack)
            {
                SendOtp();
            }
        }

        private void SendOtp()
        {
            OtpIssue issue = _passwordChange.StartVerification(UserSession.Email);
            Session[OtpSessionKey] = issue.Code;

            if (!issue.EmailSent)
            {
                lblMessage.Text = "Unable to send verification e-mail.";
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            object stored = Session[OtpSessionKey];

            if (stored is int && _passwordChange.IsCodeCorrect(txtOtp.Text, (int)stored))
            {
                Response.Redirect("ChangePassword.aspx");
            }
            else
            {
                lblMessage.Text = "Incorrect OTP";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string accessType = UserSession.Role;
            if (accessType == "Super Admin") Response.Redirect("SuperAdmin.aspx");
            else if (accessType == "Admin") Response.Redirect("AdminPage.aspx");
            else Response.Redirect("UserHome.aspx");
        }
    }
}
