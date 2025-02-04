﻿using ClientPortal.Interfaces;
using ClientPortal.Models.MessagingModels;
using ClientPortal.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ClientPortal.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _settings;

        public MailService(IOptions<MailSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task<bool> SendAsync(MailData mailData, CancellationToken ct = default)
        {

            // Initialize a new instance of the MimeKit.MimeMessage class
            var mail = new MimeMessage();

            #region Sender / Receiver
            // Sender
            mail.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
            mail.Sender = new MailboxAddress(_settings.DisplayName, _settings.From);

            // Receiver
            mail.To.Add(MailboxAddress.Parse(mailData.To));

            // Set Reply to if specified in mail data
            //if (!string.IsNullOrEmpty(mailData.ReplyTo))
            mail.ReplyTo.Add(new MailboxAddress(_settings.DisplayName, _settings.From));

            //// BCC
            //// Check if a BCC was supplied in the request
            //if (mailData.Bcc != null)
            //{
            //    // Get only addresses where value is not null or with whitespace. x = value of address
            //    foreach (string mailAddress in mailData.Bcc.Where(x => !string.IsNullOrWhiteSpace(x)))
            //        mail.Bcc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            //}

            //// CC
            //// Check if a CC address was supplied in the request
            //if (mailData.Cc != null)
            //{
            //    foreach (string mailAddress in mailData.Cc.Where(x => !string.IsNullOrWhiteSpace(x)))
            //        mail.Cc.Add(MailboxAddress.Parse(mailAddress.Trim()));
            //}
            #endregion

            #region Content

            // Add Content to Mime Message
            var body = new BodyBuilder();
            mail.Subject = "Alarm From UMFA ClientPortal";
            body.HtmlBody = mailData.Message.Replace("\r", "<br />").Replace("\n", "<br />");
            mail.Body = body.ToMessageBody();

            #endregion

            #region Send Mail

            using var smtp = new SmtpClient();

            if (_settings.UseSSL)
            {
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect, ct);
            }
            else if (_settings.UseStartTls)
            {
                await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls, ct);
            }

            await smtp.AuthenticateAsync(_settings.UserName, _settings.Password, ct);
            await smtp.SendAsync(mail, ct);
            await smtp.DisconnectAsync(true, ct);

            return true;
            #endregion


        }
    }
}