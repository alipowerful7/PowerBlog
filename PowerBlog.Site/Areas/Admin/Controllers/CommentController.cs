using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;

namespace PowerBlog.Site.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [Area("Admin")]
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CommentController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var comments = await _context.Comments.Include(c => c.Blog).Include(c => c.User).OrderByDescending(c=>c.IsConfirmation).ToListAsync();
            return View(comments);
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var comment = await _context.Comments.Include(c => c.Blog).Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
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
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }
        public async Task<IActionResult> ConfirmComment(long? id)
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
            comment.IsConfirmation = true;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Comment", new { area = "Admin" });
        }
    }
}
