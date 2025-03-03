using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;
using PowerBlog.Site.Models.ViewModels;

namespace PowerBlog.Site.Controllers
{
    public class PriceBlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public PriceBlogController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var priceBlogs = await _context.Blogs.Where(b => b.IsPublish == true && b.Price != null).ToListAsync();
            return View(priceBlogs);
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.Include(b => b.Comments.Where(c => c.IsConfirmation == true)).ThenInclude(c => c.User).Include(b => b.Comments.Where(c => c.IsConfirmation == true)).ThenInclude(c => c.ReactionBlogs).Include(b => b.User).Include(b => b.ReactionBlogs).FirstOrDefaultAsync(b => b.Id == id && b.IsPublish == true && b.Price != null);
            if (blog == null)
            {
                return NotFound();
            }
            blog.View++;
            await _context.SaveChangesAsync();

            var ctegories = await _context.Categories.OrderByDescending(c => c.CreateDate).Take(5).ToListAsync();

            var recentBlogs = await _context.Blogs.Where(b => b.Price != null && b.Id != id&&b.IsPublish==true).OrderByDescending(b => b.CreateDate).Take(5).ToListAsync();


            var userId = HttpContext.Session.GetString("UserId");
            bool isUserAddToFavorite = false;
            bool isUserAddToOrder = false;
            if (userId != null)
            {
                isUserAddToFavorite = await _context.Favorites.AnyAsync(f => f.BlogId == blog.Id && f.UserId == long.Parse(userId));
                var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.BlogId == blog.Id && f.UserId == long.Parse(userId));
                TempData["FavoriteId"] = favorite?.Id;
                isUserAddToOrder = await _context.Orders.AnyAsync(o => o.BlogId == blog.Id && o.UserId == long.Parse(userId) && o.PaymentStatus == PaymentStatus.Pending);
                var order = await _context.Orders.Include(o => o.User).Include(o => o.Blog).FirstOrDefaultAsync(o => o.BlogId == blog.Id && o.UserId == long.Parse(userId));
                if (order?.PaymentStatus == PaymentStatus.Paid)
                {
                    TempData["IsPaid"] = true;
                }
                else
                {
                    TempData["IsPaid"] = false;
                    TempData["OrderId"] = order?.Id;
                }
            }
            if (userId == null || isUserAddToFavorite == false)
            {
                TempData["IsUserAddToFavoritePrice"] = false;
            }
            else
            {
                TempData["IsUserAddToFavoritePrice"] = true;
            }
            if (userId == null || isUserAddToOrder == false)
            {
                TempData["IsUserAddToOrderPrice"] = false;
            }
            else
            {
                TempData["IsUserAddToOrderPrice"] = true;
            }
            var blogViewModel = new BlogViewModel()
            {
                Blog = blog,
                Categories = ctegories,
                RecentBlogs = recentBlogs
            };
            return View(blogViewModel);
        }
    }
}
