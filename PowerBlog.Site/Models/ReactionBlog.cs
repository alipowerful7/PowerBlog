using System.ComponentModel.DataAnnotations;

namespace PowerBlog.Site.Models
{
    public class ReactionBlog
    {
        [Key]
        public long Id { get; set; }
        public LikeOrDisLike? LikeOrDisLike { get; set; }


        #region Relations
        public User? User { get; set; }
        public long? UserId { get; set; }
        public Blog? Blog { get; set; }
        public long? BlogId { get; set; }
        public Comment? Comment { get; set; }
        public long? CommentId { get; set; }
        #endregion
    }
}
