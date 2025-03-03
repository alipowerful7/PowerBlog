using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;

namespace PowerBlog.Site.Controllers
{
    [IsLoggedIn]
    public class UserCommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserCommentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);
            var comments = await _context.Comments.Include(c => c.Blog).Where(c => c.UserId == userId).ToListAsync();
            return View(comments);
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.Include(c => c.Blog).FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
            if (comment == null)
            {
                return NotFound();
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "UserComment", new { area = "" });
        }
    }
}
