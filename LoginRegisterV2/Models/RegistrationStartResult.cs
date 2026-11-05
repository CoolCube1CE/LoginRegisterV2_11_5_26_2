namespace LoginRegisterV2
{
    /// <summary>
    /// What happened when a registration was submitted: either it is ready for
    /// OTP verification (Pending is filled in), or Errors says what to fix.
    /// </summary>
    public class RegistrationStartResult
    {
        /// <summary>Per-field messages. Always filled in; HasErrors is false when everything is fine.</summary>
        public RegistrationErrors Errors { get; set; }

        /// <summary>The validated registration waiting for its OTP. Null when Errors has something to show.</summary>
        public PendingRegistration Pending { get; set; }

        public bool Success
        {
            get { return Pending != null; }
        }
    }
}
