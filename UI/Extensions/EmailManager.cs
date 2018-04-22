using SendGrid;
using SendGrid.Helpers.Mail;

namespace UI.Extensions
{
    public class EmailManager
    {
        public static void SendEmail(string To, string body)
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("notifications@plusb.com", "PlusB Service Desk Password Reset");
            var subject = "Forgot Password Functionality";
            var to = new EmailAddress(To, "Customer");
            var plainTextContent = body;
            var htmlContent = body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }
    }
}