using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;

namespace PowerBlog.Site.Controllers
{
    [IsLoggedIn]
    public class ReactionBlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReactionBlogController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> LikeComment(long? blogId, long? commentId)
        {
            if (blogId == null || commentId == null)
            {
                return NotFound();
            }
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);
            var isUserDisLike = await _context.ReactionBlogs.FirstOrDefaultAsync(r => r.UserId == userId && r.CommentId == commentId && r.LikeOrDisLike == LikeOrDisLike.DisLike);
            if (isUserDisLike != null)
            {
                _context.ReactionBlogs.Remove(isUserDisLike);
                await _context.SaveChangesAsync();
            }
            var reactionBlog = new ReactionBlog()
            {
                UserId = userId,
                BlogId = blogId,
                CommentId = commentId,
                LikeOrDisLike = LikeOrDisLike.Like
            };
            await _context.ReactionBlogs.AddAsync(reactionBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Blog", new { id = blogId });
        }
        public async Task<IActionResult> DisLikeComment(long? blogId, long? commentId)
        {
            if (blogId == null || commentId == null)
            {
                return NotFound();
            }
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);
            var isUserLike = await _context.ReactionBlogs.FirstOrDefaultAsync(r => r.UserId == userId && r.CommentId == commentId && r.LikeOrDisLike == LikeOrDisLike.Like);
            if (isUserLike != null)
            {
                _context.ReactionBlogs.Remove(isUserLike);
                await _context.SaveChangesAsync();
            }
            var reactionBlog = new ReactionBlog()
            {
                UserId = userId,
                BlogId = blogId,
                CommentId = commentId,
                LikeOrDisLike = LikeOrDisLike.DisLike
            };
            await _context.ReactionBlogs.AddAsync(reactionBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Blog", new { id = blogId });
        }
        public async Task<IActionResult> RemoveReaction(long? reactionId)
        {
            if (reactionId == null)
            {
                return NotFound();
            }
            var reactionBlog = await _context.ReactionBlogs.FirstOrDefaultAsync(r => r.Id == reactionId);
            if (reactionBlog == null)
            {
                return NotFound();
            }
            _context.ReactionBlogs.Remove(reactionBlog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Blog", new { id = reactionBlog.BlogId });
        }
    }
}
