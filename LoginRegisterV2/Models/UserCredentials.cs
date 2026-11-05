namespace LoginRegisterV2
{
    /// <summary>The parts of an account needed to sign in: who, the stored hash, and the role.</summary>
    public class UserCredentials
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string AccessType { get; set; }
    }
}
