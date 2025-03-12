using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;
using PowerBlog.Site.Models.ViewModels;

namespace PowerBlog.Site.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(List<Blog>? blog, bool isSearch = false)
        {
            var blogs = await _context.Blogs.Where(b => b.IsPublish == true && b.Price == null).OrderByDescending(b => b.CreateDate).ToListAsync();
            if (blog?.Count != 0)
            {
                blogs = blog;
            }
            if (isSearch == true && blog?.Count == 0)
            {
                blogs = new List<Blog>();
                TempData["ErrorMessage"] = "مقاله ای با این نام پیدا نشد!";
            }
            return View("Index", blogs);
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.Include(b => b.Comments!.Where(c => c.IsConfirmation == true)).ThenInclude(c => c.User).Include(b => b.Comments!.Where(c => c.IsConfirmation == true)).ThenInclude(c => c.ReactionBlogs).Include(b => b.User).Include(b => b.ReactionBlogs).FirstOrDefaultAsync(b => b.Id == id && b.IsPublish == true);
            if (blog == null)
            {
                return NotFound();
            }
            blog.View++;
            await _context.SaveChangesAsync();

            var recentBlog = await _context.Blogs.Where(r => r.IsPublish == true && r.Id != blog.Id).OrderByDescending(r => r.UpdateDate).Take(5).ToListAsync();

            var ctegories = await _context.Categories.Include(c => c.Blogs).OrderByDescending(c => c.CreateDate).Take(5).ToListAsync();

            var relateBlog = await _context.Blogs.Where(b => b.CategoryId == blog.CategoryId && b.IsPublish == true && b.Id != blog.Id).Take(5).ToListAsync();

            var userId = HttpContext.Session.GetString("UserId");
            if (blog.Price != null && userId == null)
            {
                return RedirectToAction("Details", "PriceBlog", new { id = blog.Id });
            }
            bool isUserAddToFavorite = false;
            //bool isUserReaction = false;
            var userReaction = new List<ReactionBlog>();
            if (userId != null)
            {
                isUserAddToFavorite = await _context.Favorites.AnyAsync(f => f.BlogId == blog.Id && f.UserId == long.Parse(userId));

                //userReaction = await _context.ReactionBlogs.Include(r => r.Blog).Include(b => b.Comment).Where(r => r.UserId == long.Parse(userId) && r.BlogId == blog.Id).ToListAsync();

                var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.BlogId == blog.Id && f.UserId == long.Parse(userId));
                TempData["FavoriteId"] = favorite?.Id.ToString();
                var isUserPaid = await _context.Orders.AnyAsync(o => o.UserId == long.Parse(userId) && o.PaymentStatus == PaymentStatus.Paid && o.BlogId == blog.Id);
                if (blog.Price != null && isUserPaid == false)
                {
                    return RedirectToAction("Details", "PriceBlog", new { id = blog.Id });
                }
            }
            if (userId == null || isUserAddToFavorite == false)
            {
                TempData["IsUserAddToFavorite"] = false;
            }
            else
            {
                TempData["IsUserAddToFavorite"] = true;
            }
            var blogViewModel = new BlogViewModel()
            {
                Blog = blog,
                RecentBlogs = recentBlog,
                Categories = ctegories,
                RelateBlog = relateBlog,
                //ReactionBlogs = userReaction
            };
            return View(blogViewModel);
        }
        public async Task<IActionResult> BlogOfCategory(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.Include(c => c.Blogs!.Where(b => b.IsPublish == true)).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var blogs = category.Blogs;
            var blogOfCategoryViewModel = new BlogOfCategoryViewModel()
            {
                Category = category,
                Blogs = blogs
            };
            return View(blogOfCategoryViewModel);
        }
        public async Task<IActionResult> Search(string? searchField)
        {
            if (searchField == null)
            {
                return RedirectToAction("Index", "Blog");
            }
            var blogs = await _context.Blogs.Where(b => b.Title!.ToLower().Contains(searchField.ToLower()) && b.IsPublish == true).ToListAsync();
            return await Index(blogs, true);
        }
        public async Task<IActionResult> AddToFavorite(long? blogId)
        {
            if (blogId == null)
            {
                return NotFound();
            }
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                TempData["ErrorMessage"] = "برای افزودن به لیست علاقه مندی باید وارد شوید!";
                return RedirectToAction("Details", "Blog", new { id = blogId });
            }
            var favorite = new Favorite()
            {
                BlogId = blogId,
                UserId = long.Parse(userId)
            };
            await _context.Favorites.AddAsync(favorite);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Blog", new { id = blogId });
        }
        public async Task<IActionResult> RemoveFromFavorite(long? favoriteId)
        {
            if (favoriteId == null)
            {
                return NotFound();
            }
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.Id == favoriteId);
            if (favorite == null)
            {
                return NotFound();
            }
            var blogId = favorite.BlogId;
            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Blog", new { id = blogId });
        }
    }
}
