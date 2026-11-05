using System;

namespace LoginRegisterV2
{
    /// <summary>A full account row, as it is inserted into the "admin" table.</summary>
    [Serializable]
    public class UserAccount
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string AccessType { get; set; }
        public string PasswordHash { get; set; }
    }
}
