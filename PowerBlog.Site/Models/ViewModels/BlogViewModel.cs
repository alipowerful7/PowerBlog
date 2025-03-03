namespace PowerBlog.Site.Models.ViewModels
{
    public class BlogViewModel
    {
        public Blog? Blog { get; set; }
        public IEnumerable<Blog>? RecentBlogs { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Blog>? RelateBlog { get; set; }
        public Comment? Comment { get; set; }
        public Order? Order { get; set; }
    }
}
