namespace LoginRegisterV2
{
    /// <summary>A one-time code that was just made, and whether the e-mail carrying it was sent.</summary>
    public class OtpIssue
    {
        public int Code { get; set; }
        public bool EmailSent { get; set; }
    }
}
