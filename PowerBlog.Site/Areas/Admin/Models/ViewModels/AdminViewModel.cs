using PowerBlog.Site.Models;

namespace PowerBlog.Site.Areas.Admin.Models.ViewModels
{
    public class AdminViewModel
    {
        public long? AllBlog { get; set; }
        public long? PriceBlogs { get; set; }
        public long? UnPublishPriceBlogs { get; set; }
        public long? UnPublishBlogs { get; set; }
        public long? PublishBlogs { get; set; }
        public decimal? TodaySell { get; set; }
        public long? AllComment { get; set; }
        public long? ConfirmComment { get; set; }
        public long? UnConfirmComment { get; set; }
        public long? AllUser { get; set; }
        public Blog? MostViewBlog { get; set; }
        public Blog? MostExpensiveBlog { get; set; }
        public Blog? CheapestBlog { get; set; }
    }
}
