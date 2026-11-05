using System;

namespace LoginRegisterV2
{
    /// <summary>Registration OTP page: passes the typed code to RegistrationService, which saves the account if it is right.</summary>
    public partial class RegisterOTP : System.Web.UI.Page
    {
        private readonly RegistrationService _registration = new RegistrationService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[PendingRegistration.SessionKey] == null)
            {
                Response.Redirect("Register.aspx");
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            PendingRegistration pending = Session[PendingRegistration.SessionKey] as PendingRegistration;
            if (pending == null)
            {
                Response.Redirect("Register.aspx");
                return;
            }

            OperationResult result;
            try
            {
                result = _registration.Complete(pending, txtOtp.Text);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error saving account: " + ex.Message;
                return;
            }

            if (!result.Success)
            {
                lblMessage.Text = result.Error;
                return;
            }

            // Clear pending registration data
            Session.Remove(PendingRegistration.SessionKey);
            Response.Redirect("Login.aspx");
        }
    }
}
