using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;
using PowerBlog.Site.Utilities;

namespace PowerBlog.Site.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [Area("Admin")]
    public class SendEmailToUserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public SendEmailToUserController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostSendEmailToUser(string? subject, string? message)
        {
            if (message == null || subject == null)
            {
                return NotFound();
            }
            var users = await _context.Users.Where(u => u.UserRole == UserRole.NormalUser).ToListAsync();
            var sendEmailToUser = new SendEmailToUser(_configuration);
            var count = 0;
            foreach (var user in users)
            {
                var result = await sendEmailToUser.SendEmail(user.Email!, subject, message);
                if (result)
                {
                    count += 1;
                }
            }
            TempData["SuccessMessage"] = $"پیام با موفقیت به {count} کاربر ارسال شد.";
            return RedirectToAction("Index", "SendEmailToUser", new { area = "Admin" });
        }
    }
}
