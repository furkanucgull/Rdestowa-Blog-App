using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Services.Services.Contcrete
{
    public class PasswordResetService
    {
        private readonly EmailService _emailService;

        public PasswordResetService(EmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task SendPasswordResetEmail(string userEmail, string resetToken)
        {
            string subject = "PasswordReset";
            string resetLink = $"https://siliconmarket.com.tr/Auth/ResetPassword/{resetToken}";
            string message = $"To reset your password securely, please click the following link: <a href='{resetLink}'>Reset Password</a>";

            await _emailService.SendEmailAsync(userEmail, subject, message);
        }
    }
}
