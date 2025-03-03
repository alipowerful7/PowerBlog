using PowerBlog.Site.Models;

namespace PowerBlog.Site.Areas.Admin.Models.ViewModels
{
    public class BlogViewModel
    {
        public Blog? Blog { get; set; }
        public IEnumerable<User>? Authors { get; set; }
        public IEnumerable<Category>? Categories { get; set; }
    }
}
