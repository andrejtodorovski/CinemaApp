using CinemaApp.Domain.DomainModels;
using CinemaApp.Domain.DTO;
using CinemaApp.Repository.Interface;
using CinemaApp.Service.Interface;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;
        private readonly IRepository<EmailMessage> _mailRepository;

        public EmailService(EmailSettings settings)
        {
            _settings = settings;
        }
        public async Task SendEmailAsync(EmailMessage mail)
        {
            List<MimeMessage> messages = new List<MimeMessage>();

            
            
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_settings.SendersName, _settings.SmtpUserName),
                Subject = mail.Subject
            };
            emailMessage.From.Add(new MailboxAddress(_settings.EmailDisplayName, _settings.SmtpUserName));

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = mail.Content };

            emailMessage.To.Add(new MailboxAddress(mail.MailTo, mail.MailTo));

            messages.Add(emailMessage);
            

            try
            {
                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    var socketOptions = _settings.EnableSsl ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
                    await smtp.ConnectAsync(_settings.SmtpServer, _settings.SmtpServerPort, socketOptions);

                    if (!string.IsNullOrEmpty(_settings.SmtpUserName))
                    {
                        await smtp.AuthenticateAsync(_settings.SmtpUserName, _settings.SmtpPassword);
                    }

                    foreach (var item in messages)
                    {
                        await smtp.SendAsync(item);
                    }

                    await smtp.DisconnectAsync(true);
                }
            }
            catch (SmtpException ex)
            {
                throw ex;
            }
        }

    }
}
