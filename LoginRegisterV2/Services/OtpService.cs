using System;
using System.Security.Cryptography;

namespace LoginRegisterV2
{
    /// <summary>
    /// The rules for one-time codes: how a code is made and how it is checked.
    /// It does not send e-mail and does not touch Session.
    /// </summary>
    public class OtpService
    {
        /// <summary>Makes a 6-digit code (100000-999999) from a cryptographically secure random source.</summary>
        public int Generate()
        {
            byte[] bytes = new byte[4];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }

            uint value = BitConverter.ToUInt32(bytes, 0);
            return (int)(value % 900000u) + 100000;
        }

        /// <summary>True if what the user typed is the code that was issued.</summary>
        public bool Matches(string enteredCode, int expectedCode)
        {
            return enteredCode != null && enteredCode.Trim() == expectedCode.ToString();
        }
    }
}
