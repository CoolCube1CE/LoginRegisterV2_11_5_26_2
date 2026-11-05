namespace LoginRegisterV2
{
    /// <summary>
    /// The registration workflow, in two steps:
    ///   Start    - check the form, check the e-mail is free, e-mail an OTP, prepare the pending record
    ///   Complete - check the OTP the user typed and save the account
    /// It never touches Session or controls; the pages do that.
    /// </summary>
    public class RegistrationService
    {
        private readonly RegistrationValidator _validator = new RegistrationValidator();
        private readonly AccountService _accounts = new AccountService();
        private readonly OtpService _otp = new OtpService();
        private readonly OtpMailer _mailer = new OtpMailer();

        public RegistrationStartResult Start(RegistrationInput input)
        {
            RegistrationErrors errors = _validator.Validate(input);
            if (errors.HasErrors)
            {
                return new RegistrationStartResult { Errors = errors };
            }

            string email = input.Email.Trim();

            if (_accounts.EmailIsTaken(email))
            {
                errors.Email = "Email is registered already, please use a different e-mail address.";
                return new RegistrationStartResult { Errors = errors };
            }

            int otp = _otp.Generate();

            bool emailSent = _mailer.Send(
                email, otp,
                "Registration Verification",
                "Please enter this code in the registration form to verify your e-mail address.");

            if (!emailSent)
            {
                errors.General = "Registration cannot continue because the verification e-mail could not be sent.";
                return new RegistrationStartResult { Errors = errors };
            }

            return new RegistrationStartResult
            {
                Errors = errors,
                Pending = new PendingRegistration
                {
                    Otp = otp,
                    Account = new UserAccount
                    {
                        FullName = input.FullName.Trim(),
                        BirthDate = input.BirthDate.Value,
                        Address = input.Address.Trim(),
                        Email = email,
                        Number = input.Number.Trim(),
                        AccessType = input.AccessType,
                        PasswordHash = PasswordHelper.HashPassword(input.Password)
                    }
                }
            };
        }

        /// <summary>Saves the account if the typed code is the one that was e-mailed.</summary>
        public OperationResult Complete(PendingRegistration pending, string enteredCode)
        {
            if (!_otp.Matches(enteredCode, pending.Otp))
            {
                return OperationResult.Fail("Incorrect OTP");
            }

            _accounts.CreateAccount(pending.Account);
            return OperationResult.Ok();
        }
    }
}
