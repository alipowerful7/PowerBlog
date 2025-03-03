using System.ComponentModel.DataAnnotations;

namespace PowerBlog.Site.Models
{
    public class User
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string? FirstName { get; set; }
        [Required]
        [MaxLength(300)]
        public string? LastName { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Phone { get; set; }
        public string? Photo { get; set; }
        [Required]
        public UserRole UserRole { get; set; }
        public DateTime? CreateDate { get; set; }
        [Required]
        [MaxLength(250)]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        public decimal? Wallet { get; set; }


        #region Relations
        public ICollection<Blog>? Blogs { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<ReactionBlog>? ReactionBlogs { get; set; }
        #endregion
    }
}
