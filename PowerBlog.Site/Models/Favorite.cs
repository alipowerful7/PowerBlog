using System.ComponentModel.DataAnnotations;

namespace PowerBlog.Site.Models
{
    public class Favorite
    {
        [Key]
        public long Id { get; set; }


        #region Relations
        public User? User { get; set; }
        public long? UserId { get; set; }
        public Blog? Blog { get; set; }
        public long? BlogId { get; set; }
        #endregion
    }
}
