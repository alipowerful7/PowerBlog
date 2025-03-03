using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Areas.Admin.Models.ViewModels;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;

namespace PowerBlog.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var allBlog = await _context.Blogs.CountAsync();
            var priceBlogs = await _context.Blogs.Where(b => b.Price != null).CountAsync();
            var unPublishPriceBlogs = await _context.Blogs.Where(b => b.Price != null && b.IsPublish == false).CountAsync();
            var unPublishBlogs = await _context.Blogs.Where(b => b.IsPublish == false).CountAsync();
            var publishBlogs = await _context.Blogs.Where(b => b.IsPublish == true).CountAsync();
            var todaySell = await _context.Orders.Include(o => o.Blog).Where(o => o.PayDate.Value.Date == DateTime.Now.Date).SumAsync(o => o.Blog.Price);
            var allComment = await _context.Comments.CountAsync();
            var confirmComment = await _context.Comments.Where(c => c.IsConfirmation == true).CountAsync();
            var unConfirmComment = await _context.Comments.Where(c => c.IsConfirmation == false).CountAsync();
            var allUser = await _context.Users.CountAsync();
            var mostViewBlog = await _context.Blogs.Include(b => b.User).Include(b => b.Category).OrderByDescending(b => b.View).Take(1).FirstOrDefaultAsync();
            var mostExpensiveBlog = await _context.Blogs.Include(b => b.User).Include(b => b.Category).OrderByDescending(b => b.Price).Take(1).FirstOrDefaultAsync();
            var cheapestBlog = await _context.Blogs.Include(b => b.User).Include(b => b.Category).Where(b => b.Price != null).OrderByDescending(b => b.Price).Reverse().Take(1).FirstOrDefaultAsync();
            var adminViewModel = new AdminViewModel()
            {
                AllBlog = allBlog,
                PriceBlogs = priceBlogs,
                UnPublishPriceBlogs = unPublishPriceBlogs,
                UnPublishBlogs = unPublishBlogs,
                PublishBlogs = publishBlogs,
                TodaySell = todaySell,
                AllComment = allComment,
                ConfirmComment = confirmComment,
                UnConfirmComment = unConfirmComment,
                AllUser = allUser,
                MostViewBlog = mostViewBlog,
                MostExpensiveBlog = mostExpensiveBlog,
                CheapestBlog = cheapestBlog
            };
            return View(adminViewModel);
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        public async Task<IActionResult> Details()
        {
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return View(user);
        }
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
    }
}
