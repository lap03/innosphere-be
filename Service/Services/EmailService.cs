using Microsoft.Extensions.Options;
using Service.Interfaces;
using Service.Models.EmailModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class EmailService : IEmailService
    {
        EmailSettings _mailSettings;

        public EmailService(IOptions<EmailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendMailAsync(CancellationToken cancellationToken, EmailModel emailRequest)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient(_mailSettings.Provider, _mailSettings.Port);
                smtpClient.Credentials = new NetworkCredential(_mailSettings.DefaultSender, _mailSettings.Password);
                smtpClient.UseDefaultCredentials = false;
                smtpClient.EnableSsl = true;

                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress(_mailSettings.DefaultSender);
                mailMessage.To.Add(emailRequest.To);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = emailRequest.Subject;
                mailMessage.Body = emailRequest.Body;

                if (emailRequest.AttachmentFilePaths.Length > 0)
                {
                    foreach (var filePath in emailRequest.AttachmentFilePaths)
                    {
                        Attachment attachment = new Attachment(filePath);

                        mailMessage.Attachments.Add(attachment);
                    }
                }

                await smtpClient.SendMailAsync(mailMessage, cancellationToken);

                mailMessage.Dispose();
            }
            catch (Exception ex)
            {
                //log
                throw;
            }
        }
    }
}
