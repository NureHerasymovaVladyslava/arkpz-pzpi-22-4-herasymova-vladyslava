using Core.Configurations;
using Core.Enums;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WebAPI.Managers
{
    public class EmailManager
    {
        private readonly ILogger<EmailManager> _logger;
        private readonly EmailSettings _settings;

        private readonly Dictionary<EmailType, string> _emailBody = new()
        {
            { EmailType.NewUser,
                @"<html>
                    <h2>Dear {0}!</h2></hr>
                    <p>You were registered in ArchiveManagementSystem!</p>
                    <p>Bellow is your temporary password for accessing your account. 
                    It is advisable that you do not share it with anyone and change it as soon as possible to guarantee the security of your account.</p>
                    <p>Your temporary password: {1}</p>
                    </hr>
                    <i>Best regardes, ArchiveManagementSystem</i>
                    </html>"
            },
            { EmailType.PasswordReset,
                @"<html>
                    <h2>Dear {0}!</h2></hr>
                    <p>You requested a password reset for your account in ArchiveManagementSystem!</p>
                    <p>Bellow is your new temporary password for accessing your account. 
                    It is advisable that you do not share it with anyone and change it as soon as possible to guarantee the security of your account.</p>
                    <p>Your new temporary password: {1}</p>
                    </hr>
                    <i>Best regardes, ArchiveManagementSystem</i>
                    </html>"
            },
        };

        private readonly Dictionary<EmailType, string> _emailSubject = new()
        {
            { EmailType.NewUser, "Welcome to ArchiveManagementSystem"},
            { EmailType.PasswordReset, "Password reset requested"},
        };

        public EmailManager(IOptions<EmailSettings> settings, ILogger<EmailManager> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<bool> SendNewUserEmail(string fullName, string tempPassword, string userEmail)
        {
            var emailBody = string.Format(_emailBody[EmailType.NewUser], fullName, tempPassword);
            var emailSubject = _emailSubject[EmailType.NewUser];

            var email = FormEmailAsync(userEmail, emailSubject, emailBody);
            if (email == null)
            {
                return false;
            }

            return await SendEmailAsync(email);
        }

        public async Task<bool> SendPasswordResetEmail(string fullName, string tempPassword, string userEmail)
        {
            var emailBody = string.Format(_emailBody[EmailType.PasswordReset], fullName, tempPassword);
            var emailSubject = _emailSubject[EmailType.PasswordReset];

            var email = FormEmailAsync(userEmail, emailSubject, emailBody);
            if (email == null)
            {
                return false;
            }

            return await SendEmailAsync(email);
        }

        private MimeMessage FormEmailAsync(string toEmail, string subject, string body)
        {
            _logger.LogInformation("Forming email");

            if (string.IsNullOrWhiteSpace(toEmail))
            {
                _logger.LogError("Failed to form email - no destination was given");

                return null;
            }

            if (string.IsNullOrWhiteSpace(subject))
            {
                _logger.LogError("Failed to form email - no subject was given");

                return null;
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                _logger.LogError("Failed to form email - no body was given");

                return null;
            }

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_settings.DisplayName, _settings.From));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("html")
            {
                Text = body
            };

            _logger.LogInformation("Email formed successfully");

            return message;
        }

        private async Task<bool> SendEmailAsync(MimeMessage message)
        {
            _logger.LogInformation("Sending email: {subject}", message.Subject);

            var smtp = new SmtpClient();

            try
            {
                if (_settings.UseStartTls)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.StartTls);
                }
                else if (_settings.UseSSL)
                {
                    await smtp.ConnectAsync(_settings.Host, _settings.Port, SecureSocketOptions.SslOnConnect);
                }


                await smtp.AuthenticateAsync(_settings.UserName, _settings.Password);
                await smtp.SendAsync(message);

                _logger.LogInformation("Email sent successfully");

                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email! Error: {error}", ex.Message);

                return false;
            }
            finally
            {
                smtp.Dispose();
            }
        }
    }
}
