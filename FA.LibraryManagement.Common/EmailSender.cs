using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using PostmarkDotNet;

namespace FA.LibraryManagement.Common
{
    public class EmailSender : IEmailSender
    {
        public string PostmarkSecret { get; set; }

        public EmailSender(IConfiguration _config)
        {
            PostmarkSecret = _config.GetValue<string>("Postmark:SecretKey");
        }

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new PostmarkClient(PostmarkSecret);

            // Send an email asynchronously:
            var message = new PostmarkMessage()
            {
                To = email,
                From = "Toshokan <noreply@toshokan.xyz>",
                TrackOpens = true,
                Subject = subject,
                HtmlBody = htmlMessage
            };

            // Check if send email is successful
            var sendResult = client.SendMessageAsync(message);
            if (sendResult.Result.Status == PostmarkStatus.Success)
            {
                return Task.CompletedTask;
            }
            else
            {
                return Task.FromException(new Exception("Failed to send email"));
            }
        }
    }
}
