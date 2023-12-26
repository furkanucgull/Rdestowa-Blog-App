using Ads.Data.Services.Abstract;
using App.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ads.Data.Entities
{
    public class UserEntity : IAuditEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, EmailAddress, MaxLength(200)]
        public string Email { get; set; } = string.Empty;

        [Required, DataType(DataType.Password), MaxLength(100)]
        public string Password { get; set; } = string.Empty;
        //public byte[] PasswordHash { get; set; } = new byte[32];
        //public byte[] PasswordSalt { get; set; } = new byte[32];
        public DateTimeOffset? ResetTokenExpires { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string InstagramAddress { get; set; } = string.Empty;

        [Required, MaxLength(200)]
        public string FacebookAddress { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
        public bool IsEmailConfirmed { get; set; }
        public string Roles { get; set; } = string.Empty;
        public string? EmailConfirmationToken { get; set; } = string.Empty;
        public string? PasswordResetToken { get; set; } = string.Empty;
        public string? UserImagePath { get; set; }
        public List<UserImageEntity> UserImages { get; set; } = new List<UserImageEntity>();

        public List<CommentEntity> Comments { get; set; } = new List<CommentEntity>();
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
