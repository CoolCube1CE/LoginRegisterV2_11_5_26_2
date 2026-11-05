using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace LoginRegisterV2
{
    /// <summary>
    /// Sends one plain-text e-mail through Gmail SMTP using the sender account
    /// in Web.config (appSettings keys SmtpSenderEmail and SmtpSenderPassword).
    /// It knows nothing about OTPs or what the message says.
    /// </summary>
    public class EmailSender
    {
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;

        /// <summary>Returns true if the message was handed to the SMTP server, false if anything failed.</summary>
        public bool Send(string recipientEmail, string subject, string body)
        {
            try
            {
                string senderEmail = ConfigurationManager.AppSettings["SmtpSenderEmail"];
                string senderPassword = ConfigurationManager.AppSettings["SmtpSenderPassword"];

                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress(senderEmail);
                    message.To.Add(recipientEmail);
                    message.Subject = subject;
                    message.Body = body;

                    using (SmtpClient smtp = new SmtpClient(SmtpHost, SmtpPort))
                    {
                        smtp.EnableSsl = true;
                        smtp.Credentials = new NetworkCredential(senderEmail, senderPassword);
                        smtp.Send(message);
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
