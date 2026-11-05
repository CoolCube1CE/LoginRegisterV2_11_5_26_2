using System;

namespace LoginRegisterV2
{
    /// <summary>Raw values the user typed into the registration form (nothing checked yet).</summary>
    public class RegistrationInput
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Number { get; set; }

        /// <summary>Null when the birth date could not be read from the form.</summary>
        public DateTime? BirthDate { get; set; }

        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string AccessType { get; set; }
        public bool AgreedToPrivacy { get; set; }
    }
}
