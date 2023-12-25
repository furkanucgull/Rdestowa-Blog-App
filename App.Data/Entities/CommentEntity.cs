using Ads.Data.Entities;
using Ads.Data.Services.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class CommentEntity : IAuditEntity
    {
        [Key]
        public int CommentID { get; set; }

        [Required]
        public string Content { get; set; }

        public int UserID { get; set; }
        public UserEntity User { get; set; }

        public int PostID { get; set; }
        public BlogEntity Post { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset DeletedAt { get; set; }
    }
}
