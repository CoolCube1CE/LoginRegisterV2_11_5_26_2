namespace LoginRegisterV2
{
    /// <summary>One error message per registration field. A null or empty value means that field is fine.</summary>
    public class RegistrationErrors
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }
        public string BirthDate { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string AccessType { get; set; }
        public string General { get; set; }

        public bool HasErrors
        {
            get
            {
                return !string.IsNullOrEmpty(FullName)
                    || !string.IsNullOrEmpty(Address)
                    || !string.IsNullOrEmpty(Email)
                    || !string.IsNullOrEmpty(Number)
                    || !string.IsNullOrEmpty(BirthDate)
                    || !string.IsNullOrEmpty(Password)
                    || !string.IsNullOrEmpty(ConfirmPassword)
                    || !string.IsNullOrEmpty(AccessType)
                    || !string.IsNullOrEmpty(General);
            }
        }
    }
}
