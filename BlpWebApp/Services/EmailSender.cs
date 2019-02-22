﻿using BlpWebApp.Options;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit;
using System.Threading.Tasks;

namespace BlpWebApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailOptions _emailOptions;

        public EmailSender(IOptions<EmailOptions> options)
        {
            _emailOptions = options.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress(_emailOptions.From));
            message.To.Add(new MailboxAddress(email));
            message.Subject = subject;
            message.Body = new TextPart("html")
                {
                    Text = htmlMessage
                };

            using (SmtpClient client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = MailService.DefaultServerCertificateValidationCallback;
                await client.ConnectAsync(_emailOptions.Host, _emailOptions.Port ?? 0, _emailOptions.SSL);

                if(!(string.IsNullOrWhiteSpace(_emailOptions.Password)))
                {
                    await client.AuthenticateAsync(_emailOptions.UserName, _emailOptions.Password);
                }

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
