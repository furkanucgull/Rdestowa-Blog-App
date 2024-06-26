using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{

    public class UserViewModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        //[Required(ErrorMessage = "New email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string NewEmail { get; set; }

        //[Required(ErrorMessage = "Verify New email is required.")]
        [Compare("NewEmail", ErrorMessage = "The Verify New email must match the New email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string VerifyNewEmail { get; set; }
        public string? Password { get; set; }
        public string? PasswordVerify { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string FacebookAddress { get; set; } = string.Empty;
        public string InstagramAddress { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public string UserImagePath { get; set; }
    }

}
