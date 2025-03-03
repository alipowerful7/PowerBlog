using System.ComponentModel.DataAnnotations;

namespace PowerBlog.Site.Models
{
    public class Blog
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string? Title { get; set; }
        [MaxLength(400)]
        public string? ShortDescription { get; set; }
        [Required]
        public string? TextBody { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
        public bool IsPublish { get; set; }
        public string? Photo { get; set; }
        public long View { get; set; }
        public decimal? Price { get; set; }


        #region Relations
        public User? User { get; set; }
        public long? UserId { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public Category? Category { get; set; }
        public long? CategoryId { get; set; }
        public ICollection<Favorite>? Favorites { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public ICollection<ReactionBlog>? ReactionBlogs { get; set; }
        #endregion
    }
}
