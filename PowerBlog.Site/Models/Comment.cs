using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PowerBlog.Site.Models
{
    public class Comment
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string? Title { get; set; }
        [Required]
        public string? TextBody { get; set; }
        public DateTime? CreateDate { get; set; }
        public bool IsConfirmation { get; set; }


        #region Relations
        public User? User { get; set; }
        public long? UserId { get; set; }
        public Blog? Blog { get; set; }
        public long? BlogId { get; set; }
        public ICollection<ReactionBlog>? ReactionBlogs { get; set; }
        #endregion
    }
}
