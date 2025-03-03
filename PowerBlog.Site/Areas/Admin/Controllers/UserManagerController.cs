using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;
using PowerBlog.Site.Utilities;

namespace PowerBlog.Site.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class UserManagerController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserManagerController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = await _context.Users.OrderByDescending(u => u.UserRole).ToListAsync();
            return View(users);
        }
        public IActionResult Creat()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostCreat(User user, IFormFile? ImageFile)
        {
            if (ImageFile != null)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/UserImages");
                string filePath = Path.Combine(folderPath, fileName);
                Directory.CreateDirectory(folderPath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                user.Photo = fileName;
            }
            user.CreateDate = DateTime.Now;
            user.Password = user.Password?.HashPassword();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(long? id)
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
            var userRoleName = (UserRole)user.UserRole;
            ViewData["UserRoleName"] = userRoleName;
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
        [HttpPost]
        public async Task<IActionResult> PostEdit(User postUser, IFormFile? ImageFile)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == postUser.Id);
            if (user == null)
            {
                return NotFound();
            }
            if (ImageFile != null)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/UserImages");
                string filePath = Path.Combine(folderPath, fileName);
                Directory.CreateDirectory(folderPath);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
                user.Photo = fileName;
            }
            user.FirstName = postUser.FirstName;
            user.LastName = postUser.LastName;
            user.Email = postUser.Email;
            user.Phone = postUser.Phone;
            user.UserRole = postUser.UserRole;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(long? id)
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
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
