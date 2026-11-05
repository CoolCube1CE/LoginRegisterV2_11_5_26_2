using System;
using System.Text.RegularExpressions;

namespace LoginRegisterV2
{
    /// <summary>
    /// Checks the registration form values and returns one message per bad field.
    /// It does not read controls, write labels, or touch the database.
    /// </summary>
    public class RegistrationValidator
    {
        private const int MinLength = 5;

        public RegistrationErrors Validate(RegistrationInput input)
        {
            RegistrationErrors errors = new RegistrationErrors();

            errors.FullName = CheckFullName(input.FullName);
            errors.Address = CheckAddress(input.Address);
            errors.Email = CheckEmail(input.Email);
            errors.Number = CheckPhoneNumber(input.Number);
            errors.BirthDate = CheckBirthDate(input.BirthDate);
            errors.Password = CheckPassword(input.Password);
            errors.ConfirmPassword = CheckConfirmPassword(input.Password, input.ConfirmPassword);
            errors.AccessType = CheckAccessType(input.AccessType);

            // Checked on the server too, because disabling the button in the browser can be bypassed.
            errors.General = input.AgreedToPrivacy
                ? null
                : "You must agree to the Data Privacy Terms and Agreement before registering.";

            return errors;
        }

        // Full name: letters, spaces, periods, commas only
        private static string CheckFullName(string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
                return "Full Name is required";
            if (!Regex.IsMatch(fullName, @"^[a-zA-Z ,.]+$"))
                return "Invalid, must only contain letters, periods, and commas";
            if (fullName.Length < MinLength)
                return "Full Name is too short";
            return null;
        }

        private static string CheckAddress(string address)
        {
            if (string.IsNullOrEmpty(address))
                return "Address is required";
            if (!Regex.IsMatch(address, @"^[a-zA-Z0-9 !#$%&'*+\-/=?^_`{|}~,]+$"))
                return "Invalid address";
            if (address.Length < MinLength)
                return "Address is too short";
            return null;
        }

        private static string CheckEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return "E-mail is required";
            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9.!#$%&'*+\-/=?^_`{|}~]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                return "Invalid E-mail";
            return null;
        }

        private static string CheckPhoneNumber(string number)
        {
            if (string.IsNullOrEmpty(number))
                return "Phone Number is required";
            if (!Regex.IsMatch(number, @"^09[0-9]{9}$") && !Regex.IsMatch(number, @"^\+639[0-9]{9}$"))
                return "Invalid Phone Number. Must be 09XXXXXXXXX or +639XXXXXXXXX";
            return null;
        }

        private static string CheckBirthDate(DateTime? birthDate)
        {
            if (!birthDate.HasValue)
                return "Birth date is required";
            if (!Is18OrOlder(birthDate.Value))
                return "You must be at least 18 years old.";
            return null;
        }

        private static string CheckPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return "Password is required";
            if (!PasswordPolicy.IsStrong(password))
                return "Password does not meet the requirements above";
            return null;
        }

        private static string CheckConfirmPassword(string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(confirmPassword))
                return "Confirm Password Required";
            if (password != confirmPassword)
                return "Password does not match";
            return null;
        }

        private static string CheckAccessType(string accessType)
        {
            if (string.IsNullOrEmpty(accessType))
                return "Access Type is required";
            return null;
        }

        private static bool Is18OrOlder(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age >= 18;
        }
    }
}
