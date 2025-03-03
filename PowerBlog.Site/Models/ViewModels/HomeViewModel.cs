namespace PowerBlog.Site.Models.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Blog>? Blogs { get; set; }
    }
}
