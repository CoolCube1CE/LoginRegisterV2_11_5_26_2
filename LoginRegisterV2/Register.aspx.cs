using System;

namespace LoginRegisterV2
{
    /// <summary>
    /// Registration page: reads the form, shows errors, and moves to the OTP page.
    /// The whole registration workflow lives in RegistrationService.
    /// </summary>
    public partial class Register : System.Web.UI.Page
    {
        private readonly RegistrationService _registration = new RegistrationService();

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            RegistrationInput input = ReadForm();

            RegistrationStartResult result;
            try
            {
                result = _registration.Start(input);
            }
            catch (Exception ex)
            {
                ShowErrors(new RegistrationErrors());
                lblGeneralError.Text = "Error: " + ex.Message;
                return;
            }

            ShowErrors(result.Errors);
            if (!result.Success) return;

            Session[PendingRegistration.SessionKey] = result.Pending;
            Response.Redirect("RegisterOTP.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }

        // ------------------------------------------------------------------
        // Form <-> objects
        // ------------------------------------------------------------------

        private RegistrationInput ReadForm()
        {
            return new RegistrationInput
            {
                FullName = txtFullName.Text,
                Address = txtAddress.Text,
                Email = txtEmail.Text,
                Number = txtNumber.Text,
                BirthDate = ReadBirthDate(),
                Password = txtPassword.Text,
                ConfirmPassword = txtConfirmPassword.Text,
                AccessType = ddlAccessType.SelectedValue,
                AgreedToPrivacy = chkDataPrivacy.Checked
            };
        }

        // Birth date comes from either the calendar textbox or the 3-part manual entry.
        private DateTime? ReadBirthDate()
        {
            if (hdnBirthdateMode.Value == "manual")
            {
                int year, month, day;
                if (int.TryParse(txtYear.Text, out year) &&
                    int.TryParse(txtMonth.Text, out month) &&
                    int.TryParse(txtDay.Text, out day))
                {
                    try { return new DateTime(year, month, day); }
                    catch (ArgumentOutOfRangeException) { return null; }
                }
                return null;
            }

            DateTime parsed;
            if (DateTime.TryParse(txtBirthDate.Text, out parsed))
            {
                return parsed;
            }
            return null;
        }

        private void ShowErrors(RegistrationErrors errors)
        {
            lblFullNameError.Text = errors.FullName ?? "";
            lblAddressError.Text = errors.Address ?? "";
            lblEmailError.Text = errors.Email ?? "";
            lblNumberError.Text = errors.Number ?? "";
            lblBirthDateError.Text = errors.BirthDate ?? "";
            lblPasswordError.Text = errors.Password ?? "";
            lblConfirmPasswordError.Text = errors.ConfirmPassword ?? "";
            lblAccessTypeError.Text = errors.AccessType ?? "";
            lblGeneralError.Text = errors.General ?? "";
        }
    }
}
