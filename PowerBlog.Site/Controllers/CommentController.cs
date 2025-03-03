using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;

namespace PowerBlog.Site.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public async Task<IActionResult> PostComment(Comment comment)
        {
            var user = HttpContext.Session.GetString("UserId");
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == comment.BlogId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "برای ارسال نظر ابتدا وارد حساب کاربری خود شوید";
                if (blog.Price != null)
                {
                    return RedirectToAction("Details", "PriceBlog", new { id = comment.BlogId });
                }
                return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
            }
            comment.CreateDate = DateTime.Now;
            comment.UserId = long.Parse(user);
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            if (blog.Price != null)
            {
                return RedirectToAction("Details", "PriceBlog", new { id = comment.BlogId });
            }
            return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
        }
    }
}
