
using BCMT_Associates.Interfaces;
using BCMT_Associates.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using static BCMT_Associates.Helpers.Helper;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;


namespace BCMT_Associates.Repository
{
    public class EmailServiceRepository : IEmailService 
	{
        private readonly AppSettings _appSettings;

        public EmailServiceRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public void Send(string to, string subject, string html, string? from = null, string? logoPath = null)
        {
            // create message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _appSettings.EmailFrom));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };
            var attachment = new MimePart("image", "jpeg")
            {
                Content = new MimeContent(File.OpenRead(logoPath)),
                ContentDisposition = new MimeKit.ContentDisposition(MimeKit.ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = Path.GetFileName(logoPath)
            };

            // now create the multipart/mixed container to hold the message text and the
            // image attachment
            var multipart = new Multipart("mixed");
            multipart.Add(email.Body);
            multipart.Add(attachment);

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_appSettings.SmtpHost, _appSettings.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_appSettings.SmtpUser, _appSettings.SmtpPass);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
