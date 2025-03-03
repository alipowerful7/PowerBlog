using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;
using PowerBlog.Site.Models.ViewModels;
using PowerBlog.Site.Utilities;

namespace PowerBlog.Site.Controllers
{
    [IsLoggedIn]
    public class UserPanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserPanelController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            var myComments = await _context.Comments.Where(c => c.UserId == userId).CountAsync();
            var userViewModel = new UserViewModel()
            {
                MyComments = myComments,
                Wallet = (user?.Wallet != null) ? user.Wallet : 0
            };
            return View(userViewModel);
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
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            if (user.UserRole == UserRole.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return RedirectToAction("Index", "UserPanel");
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostChangePassword(string? CurrentPassword, string? NewPassword)
        {
            if (CurrentPassword == null || NewPassword == null)
            {
                return NotFound();
            }
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return NotFound();
            }
            var isTrue = CurrentPassword.VerifyPassword(user.Password!);
            if (isTrue == false)
            {
                TempData["ErrorMessage"] = "رمز عبور فعلی نادرست است.";
                return RedirectToAction("ChangePassword", "UserPanel");
            }
            user.Password = NewPassword.HashPassword();
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            if (user.UserRole == UserRole.Admin)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return RedirectToAction("Index", "UserPanel");
        }
        public async Task<IActionResult> Favorites()
        {
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);
            var favorites = await _context.Favorites.Include(f => f.Blog).ThenInclude(b => b.User).Include(f => f.Blog).ThenInclude(b => b.Category).Where(f => f.UserId == userId).Select(f => f.Blog).ToListAsync();
            return View(favorites);
        }
    }
}
