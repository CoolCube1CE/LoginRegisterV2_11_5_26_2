namespace LoginRegisterV2
{
    /// <summary>Writes the "your verification code is ..." message and hands it to EmailSender.</summary>
    public class OtpMailer
    {
        private readonly EmailSender _sender = new EmailSender();

        public bool Send(string recipientEmail, int otp, string subject, string bodyIntro)
        {
            string body = bodyIntro + "\n\nYour verification code is " + otp +
                          "\n\nIf you did not request this, you can ignore this email.";

            return _sender.Send(recipientEmail, subject, body);
        }
    }
}
