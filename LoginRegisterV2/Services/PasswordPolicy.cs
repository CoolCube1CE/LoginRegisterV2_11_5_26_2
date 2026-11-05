using System.Text.RegularExpressions;

namespace LoginRegisterV2
{
    /// <summary>
    /// The rules a password must meet. One definition, used by Register and
    /// ChangePassword (it used to be copy-pasted in both pages).
    /// </summary>
    public static class PasswordPolicy
    {
        public const string RequirementsMessage =
            "Password must be 8-16 characters and include an uppercase letter, lowercase letter, number, and special character.";

        public static bool IsStrong(string password)
        {
            if (password == null)
            {
                return false;
            }

            bool length = password.Length >= 8 && password.Length <= 16;
            bool upper = Regex.IsMatch(password, @"[A-Z]");
            bool lower = Regex.IsMatch(password, @"[a-z]");
            bool number = Regex.IsMatch(password, @"[0-9]");
            bool special = Regex.IsMatch(password, @"[^a-zA-Z0-9]");
            return length && upper && lower && number && special;
        }
    }
}
