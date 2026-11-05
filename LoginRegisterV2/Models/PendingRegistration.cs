using System;

namespace LoginRegisterV2
{
    /// <summary>
    /// A registration that has been validated and had its OTP e-mailed, but is not
    /// saved yet. Stored in Session under one key (replaces the eight loose
    /// "Reg_..." session keys) until the user enters the correct OTP.
    /// </summary>
    [Serializable]
    public class PendingRegistration
    {
        public const string SessionKey = "PendingRegistration";

        public UserAccount Account { get; set; }
        public int Otp { get; set; }
    }
}
