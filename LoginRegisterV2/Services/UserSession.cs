using System.Web;
using System.Web.SessionState;

namespace LoginRegisterV2
{
    /// <summary>
    /// The one place that knows how the signed-in user is remembered between
    /// requests. Pages ask "who is signed in?" here instead of reading
    /// Session["..."] keys themselves, so the keys exist in exactly one file.
    /// </summary>
    public static class UserSession
    {
        // Same keys as before, so nothing already stored in a session breaks.
        private const string EmailKey = "UserEmail";
        private const string RoleKey = "AccessType";

        private static HttpSessionState Session
        {
            get { return HttpContext.Current.Session; }
        }

        public static bool IsSignedIn
        {
            get { return Session[EmailKey] != null; }
        }

        /// <summary>The signed-in user's e-mail, or null if nobody is signed in.</summary>
        public static string Email
        {
            get { return AsText(Session[EmailKey]); }
        }

        /// <summary>The signed-in user's access type ("User", "Admin", "Super Admin"), or null if nobody is signed in.</summary>
        public static string Role
        {
            get { return AsText(Session[RoleKey]); }
        }

        /// <summary>True only when someone is signed in AND has exactly this access type.</summary>
        public static bool IsInRole(string role)
        {
            return IsSignedIn && Role == role;
        }

        public static void SignIn(string email, string role)
        {
            Session[EmailKey] = email;
            Session[RoleKey] = role;
        }

        /// <summary>Signs the user out and clears everything else kept in their session.</summary>
        public static void SignOut()
        {
            Session.Clear();
        }

        private static string AsText(object value)
        {
            return value == null ? null : value.ToString();
        }
    }
}
