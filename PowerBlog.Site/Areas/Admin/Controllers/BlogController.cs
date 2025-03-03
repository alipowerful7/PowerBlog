using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Areas.Admin.Models.ViewModels;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;

namespace PowerBlog.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var blogs = await _context.Blogs.Include(b => b.User).Include(b => b.Category).OrderByDescending(b => b.CreateDate).ToListAsync();
            return View(blogs);
        }
        public async Task<IActionResult> Creat()
        {
            var authors = await _context.Users.Where(u => u.UserRole == UserRole.Admin || u.UserRole == UserRole.Author).ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var blogViewModel = new BlogViewModel()
            {
                Authors = authors,
                Categories = categories
            };
            return View(blogViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> PostCreat(Blog blog, IFormFile? ImageFile)
        {
            if (ImageFile != null)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/BlogImages");
                string filePath = Path.Combine(folderPath, fileName);
                Directory.CreateDirectory(folderPath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                blog.Photo = fileName;
            }
            blog.CreateDate = DateTime.Now;
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == blog.CategoryId);
            var categoryName = (category != null) ? category!.Name : null;
            ViewData["CategoryName"] = categoryName;
            return View(blog);
        }
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            var authors = await _context.Users.Where(u => u.UserRole == UserRole.Admin || u.UserRole == UserRole.Author).ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var blogViewModel = new BlogViewModel()
            {
                Authors = authors,
                Categories = categories,
                Blog = blog
            };
            return View(blogViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> PostEdit(BlogViewModel postBlog, IFormFile? ImageFile)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == postBlog.Blog.Id);
            if (blog == null)
            {
                return NotFound();
            }
            if (ImageFile != null)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/BlogImages");
                string filePath = Path.Combine(folderPath, fileName);
                Directory.CreateDirectory(folderPath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                blog.Photo = fileName;
            }
            blog.Title = postBlog.Blog.Title;
            blog.ShortDescription = postBlog.Blog.ShortDescription;
            blog.TextBody = postBlog.Blog.TextBody;
            blog.UpdateDate = postBlog.Blog.UpdateDate;
            blog.IsPublish = postBlog.Blog.IsPublish;
            blog.CategoryId = postBlog.Blog.CategoryId;
            blog.UserId = postBlog.Blog.UserId;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PublishBlog(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == id);
            if (blog == null)
            {
                return NotFound();
            }
            blog.IsPublish = true;
            _context.Blogs.Update(blog);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> PriceBlog()
        {
            var authors = await _context.Users.Where(u => u.UserRole == UserRole.Admin || u.UserRole == UserRole.Author).ToListAsync();
            var categories = await _context.Categories.ToListAsync();
            var blogViewModel = new BlogViewModel()
            {
                Authors = authors,
                Categories = categories
            };
            return View(blogViewModel);
        }
    }
}
