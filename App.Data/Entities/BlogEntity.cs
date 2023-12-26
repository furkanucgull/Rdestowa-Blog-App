﻿using App.Data.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class BlogEntity : IAuditEntity

    {
        [Key]
        public int PostID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public string Author { get; set; }

        [Required]
        public string Category { get; set; }

        public int CommentCount { get; set; }

        public int UserID { get; set; }
        public UserEntity User { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTime.Now;
        public DateTimeOffset UpdatedAt { get; set; } = DateTime.Now;
        public DateTimeOffset DeletedAt { get; set; } = DateTime.Now;
    }
}
