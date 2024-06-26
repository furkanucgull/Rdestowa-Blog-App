﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Data.Abstract;

namespace App.Data.Entities
{
    public class UserImageEntity : IAuditEntity
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required, MaxLength(200)]
        public string ImagePath { get; set; } = string.Empty;

        public int UserId { get; set; }

        public long? ImageSize { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
        // Navigation Properties
        [ForeignKey(nameof(UserId))]
        public UserEntity User{ get; set; } = null!;

    }
}
