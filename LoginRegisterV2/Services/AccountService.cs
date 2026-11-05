namespace LoginRegisterV2
{
    public enum ChangePasswordResult
    {
        Success,
        SameAsCurrent,
        AccountNotFound
    }

    /// <summary>
    /// The account rules: who may sign in, whether an e-mail is taken, and how a
    /// password is changed. It uses UserRepository for SQL and PasswordHelper for
    /// hashing, and never touches Session or controls.
    /// </summary>
    public class AccountService
    {
        private readonly UserRepository _users = new UserRepository();

        /// <summary>Returns the account's credentials if the e-mail and password are right, otherwise null.</summary>
        public UserCredentials Authenticate(string email, string password)
        {
            UserCredentials found = _users.FindCredentials(email);
            if (found == null)
            {
                return null;
            }

            return PasswordHelper.VerifyPassword(password, found.PasswordHash) ? found : null;
        }

        public bool EmailIsTaken(string email)
        {
            return _users.EmailExists(email);
        }

        public void CreateAccount(UserAccount account)
        {
            _users.Insert(account);
        }

        public ChangePasswordResult ChangePassword(string email, string newPassword)
        {
            string currentHash = _users.GetPasswordHash(email);
            if (currentHash != null && PasswordHelper.VerifyPassword(newPassword, currentHash))
            {
                return ChangePasswordResult.SameAsCurrent;
            }

            string newHash = PasswordHelper.HashPassword(newPassword);
            return _users.UpdatePasswordHash(email, newHash)
                ? ChangePasswordResult.Success
                : ChangePasswordResult.AccountNotFound;
        }
    }
}
