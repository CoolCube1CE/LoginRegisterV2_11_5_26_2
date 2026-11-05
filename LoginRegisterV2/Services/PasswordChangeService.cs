namespace LoginRegisterV2
{
    /// <summary>
    /// The change-password workflow for a signed-in user: e-mail them a code,
    /// check the code, then validate and apply the new password.
    /// It never touches Session or controls; the pages do that.
    /// </summary>
    public class PasswordChangeService
    {
        private readonly AccountService _accounts = new AccountService();
        private readonly OtpService _otp = new OtpService();
        private readonly OtpMailer _mailer = new OtpMailer();

        /// <summary>Makes a code and e-mails it to the user. The caller keeps the code to check later.</summary>
        public OtpIssue StartVerification(string email)
        {
            int code = _otp.Generate();

            bool sent = _mailer.Send(
                email, code,
                "Password Change Verification",
                "Please enter this code to continue changing your password.");

            return new OtpIssue { Code = code, EmailSent = sent };
        }

        public bool IsCodeCorrect(string enteredCode, int issuedCode)
        {
            return _otp.Matches(enteredCode, issuedCode);
        }

        /// <summary>Checks the new password against the rules, then saves it.</summary>
        public OperationResult Change(string email, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(newPassword))
                return OperationResult.Fail("New Password is required.");

            if (!PasswordPolicy.IsStrong(newPassword))
                return OperationResult.Fail(PasswordPolicy.RequirementsMessage);

            if (string.IsNullOrEmpty(confirmPassword))
                return OperationResult.Fail("Confirm Password is required.");

            if (newPassword != confirmPassword)
                return OperationResult.Fail("Password does not match.");

            switch (_accounts.ChangePassword(email, newPassword))
            {
                case ChangePasswordResult.SameAsCurrent:
                    return OperationResult.Fail("You cannot use your previous password. Please enter a new password.");

                case ChangePasswordResult.AccountNotFound:
                    return OperationResult.Fail("Unable to change password. Account was not found.");

                default:
                    return OperationResult.Ok();
            }
        }
    }
}
