using System.ComponentModel.DataAnnotations;

namespace PowerBlog.Site.Models
{
    public class Category
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [MaxLength(250)]
        public string? Name { get; set; }
        public string? Photo { get; set; }
        public DateTime? CreateDate { get; set; } = DateTime.Now;


        #region Relations
        public ICollection<Blog>? Blogs { get; set; }
        #endregion
    }
}
