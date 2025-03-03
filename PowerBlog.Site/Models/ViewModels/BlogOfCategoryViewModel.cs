namespace PowerBlog.Site.Models.ViewModels
{
    public class BlogOfCategoryViewModel
    {
        public IEnumerable<Blog>? Blogs { get; set; }
        public Category? Category { get; set; }
    }
}
