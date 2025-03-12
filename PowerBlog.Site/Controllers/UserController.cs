using Microsoft.AspNetCore.Mvc;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;
using PowerBlog.Site.Utilities;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace PowerBlog.Site.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public UserController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return RedirectToAction("SignIn");
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserSignUpValidation(User user, IFormFile? ImageFile)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == user.UserName) || await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                TempData["ErrorMessage"] = "نام کاربری یا ایمیل وجود دارد.";
                return RedirectToAction("SignUp");
            }
            TempData["User"] = JsonConvert.SerializeObject(user);
            //TempData["IFormFile"] = JsonConvert.SerializeObject(ImageFile);
            if (ImageFile != null)
            {
                var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
                if (!Directory.Exists(tempFolder))
                {
                    Directory.CreateDirectory(tempFolder);
                }

                // تولید نام فایل یکتا (استفاده از Path.GetRandomFileName)
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                var filePath = Path.Combine(tempFolder, fileName);

                // ذخیره فایل به صورت آسنکرون
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // ذخیره مسیر فایل در TempData برای استفاده در اکشن بعدی
                TempData["UploadedFilePath"] = filePath;

            }
            return RedirectToAction("SignUpEmail");
        }
        public async Task<IActionResult> SignUpEmail()
        {
            var userEmail = JsonConvert.DeserializeObject<User>(TempData["User"] as string);
            TempData.Keep();
            string email = userEmail?.Email!;

            //var emailSettings = _configuration.GetSection("EmailSettings");
            //string smtpServer = emailSettings["SmtpServer"]!;
            //int port = int.Parse(emailSettings["Port"]!);
            //string senderEmail = emailSettings["SenderEmail"]!;
            //string senderPassword = emailSettings["SenderPassword"]!;
            //bool enableSSL = bool.Parse(emailSettings["EnableSSL"]!);

            //var random = new Random();
            //string verificationCode = random.Next(100000, 999999).ToString();

            //var sendVerifyEmail = new SendVerifyEmail(_configuration);
            //await sendVerifyEmail.SendEmail(email, verificationCode);

            //var message = new MimeMessage();
            //message.From.Add(new MailboxAddress("PowerBlog", senderEmail));

            //string recipientEmail = email;
            //message.To.Add(new MailboxAddress("", recipientEmail));

            //message.Subject = "کد تایید حساب کاربری";
            //message.Body = new TextPart("html")
            //{
            //    Text = $"<h1>کد تایید شما:</h1><h3>{verificationCode}</h3>"
            //};

            //try
            //{
            //    using (var client = new SmtpClient())
            //    {
            //        await client.ConnectAsync(smtpServer, port, SecureSocketOptions.StartTls);

            //        await client.AuthenticateAsync(senderEmail, senderPassword);

            //        await client.SendAsync(message);

            //        await client.DisconnectAsync(true);
            //    }

            //    TempData["VerificationCode"] = verificationCode;
            //    return View();
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.ErrorMessage = "خطا در ارسال ایمیل: " + ex.Message;
            //    return View("Error");
            //}

            var random = new Random();
            string verificationCode = random.Next(100000, 999999).ToString();

            var sendVerifyEmail = new SendVerifyEmail(_configuration);
            var isTrue = await sendVerifyEmail.SendEmail(email, verificationCode);
            if (isTrue == false)
            {
                return NotFound();
            }
            TempData["VerificationCode"] = verificationCode;
            return View();
        }
        [HttpPost]
        public IActionResult PostSignUpEmail(string emailCode)
        {
            if (emailCode != TempData["VerificationCode"]?.ToString())
            {
                TempData["ErrorMessage"] = "کد نادرست است.\nکد دیگری مجددا به ایمیل شما ارسال شد.";
                return RedirectToAction("SignUpEmail");
            }
            return RedirectToAction("RegisterUser");
        }
        public async Task<IActionResult> RegisterUser()
        {
            //var ImageFile = JsonConvert.DeserializeObject<IFormFile>(TempData["IFormFile"] as string);
            var user = JsonConvert.DeserializeObject<User>(TempData["User"] as string);
            //if (ImageFile != null)
            //{
            //    string fileName = $"{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
            //    string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/UserImages");
            //    string filePath = Path.Combine(folderPath, fileName);
            //    Directory.CreateDirectory(folderPath);
            //    using (var stream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await ImageFile.CopyToAsync(stream);
            //    }
            //    user.Photo = fileName;
            //}
            var oldFilePath = TempData["UploadedFilePath"] as string;
            if (string.IsNullOrEmpty(oldFilePath) || !System.IO.File.Exists(oldFilePath))
            {
            }
            else
            {
                var fileName = Path.GetFileName(oldFilePath);

                // تعیین مسیر پوشه مقصد (wwwroot/images/userimages)
                var newFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images/UserImages");

                // ساخت مسیر جدید فایل
                var newFilePath = Path.Combine(newFolderPath, fileName);

                // انتقال فایل از مسیر قدیمی به مسیر جدید؛ File.Move مسیر قدیمی را حذف می‌کند
                System.IO.File.Move(oldFilePath, newFilePath);
                user!.Photo = fileName;
                System.IO.File.Delete(oldFilePath);
            }

            user!.UserRole = UserRole.NormalUser;
            user.CreateDate = DateTime.Now;
            user.Password = user.Password?.HashPassword();
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return RedirectToAction("SignIn");
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostSignIn(string? username, string? password)
        {
            if (username == null || password == null)
            {
                TempData["ErrorMessage"] = "نام کاربری یا رمز عبور نمی تواند خالی باشد.";
                return RedirectToAction("SignIn");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null || !password.VerifyPassword(user.Password!))
            {
                TempData["ErrorMessage"] = "نام کاربری یا رمز عبور اشتباه است.";
                return RedirectToAction("SignIn");
            }

            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserRole", user.UserRole.ToString());
            if (user.UserRole == UserRole.Admin || user.UserRole == UserRole.Author)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public IActionResult SignInWithEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostSignInWithEmailValidation(string? email)
        {
            if (email == null)
            {
                TempData["ErrorMessage"] = "ایمیل نمی تواند خالی باشد.";
                return RedirectToAction("SignInWithEmail", "User");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "ایمیل اشتباه است.";
                return RedirectToAction("SignInWithEmail", "User");
            }
            TempData["UserEmail"] = email;
            return RedirectToAction("SignInWithEmailCode", "User");
        }
        public async Task<IActionResult> SignInWithEmailCode()
        {
            var email = TempData["UserEmail"] as string;
            TempData.Keep();
            var random = new Random();
            string verificationCode = random.Next(100000, 999999).ToString();
            var sendVerifyEmail = new SendVerifyEmail(_configuration);
            var isTrue = await sendVerifyEmail.SendEmail(email!, verificationCode);
            if (isTrue == false)
            {
                return NotFound();
            }
            TempData["VerificationCode"] = verificationCode;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostSignInWithEmailCode(string? emailCode)
        {
            if (emailCode == null)
            {
                TempData["ErrorMessage"] = "کد تایید نمیتواند خالی باشد.";
                return RedirectToAction("SignInWithEmailCode", "User");
            }
            if (emailCode != TempData["VerificationCode"]?.ToString())
            {
                TempData["ErrorMessage"] = "کد تایید اشتباه است.";
                return RedirectToAction("SignInWithEmailCode", "User");
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == TempData["UserEmail"]!.ToString());

            HttpContext.Session.SetString("IsLoggedIn", "true");
            HttpContext.Session.SetString("UserId", user?.Id.ToString()!);
            HttpContext.Session.SetString("UserRole", user!.UserRole.ToString());
            if (user.UserRole == UserRole.Admin || user.UserRole == UserRole.Author)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostForgetPasswordValidation(string? email)
        {
            if (email == null)
            {
                TempData["ErrorMessage"] = "ایمیل نمی تواند خالی باشد.";
                return RedirectToAction("ForgetPassword", "User");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                TempData["ErrorMessage"] = "ایمیل اشتباه است.";
                return RedirectToAction("ForgetPassword", "User");
            }
            TempData["UserEmail"] = email;
            return RedirectToAction("ForgetPasswordCode", "User");
        }
        public async Task<IActionResult> ForgetPasswordCode()
        {
            var email = TempData["UserEmail"] as string;
            TempData.Keep();
            var random = new Random();
            string verificationCode = random.Next(100000, 999999).ToString();
            var sendVerifyEmail = new SendVerifyEmail(_configuration);
            var isTrue = await sendVerifyEmail.SendEmail(email!, verificationCode);
            if (isTrue == false)
            {
                return NotFound();
            }
            TempData["VerificationCode"] = verificationCode;
            return View();
        }
        [HttpPost]
        public IActionResult PostForgetPasswordCode(string? emailCode)
        {
            if (emailCode == null)
            {
                TempData["ErrorMessage"] = "کد تایید نمیتواند خالی باشد.";
                return RedirectToAction("ForgetPasswordCode", "User");
            }
            if (emailCode != TempData["VerificationCode"]?.ToString())
            {
                TempData["ErrorMessage"] = "کد تایید اشتباه است.";
                return RedirectToAction("ForgetPasswordCode", "User");
            }
            return RedirectToAction("ChangePassword", "User");
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostChangePassword(string? newPassword)
        {
            if (newPassword == null)
            {
                TempData["ErrorMessage"] = "رمز عبور نمی تواند خالی باشد.";
                return RedirectToAction("ChangePassword", "User");
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == TempData["UserEmail"]!.ToString());
            user!.Password = newPassword.HashPassword();
            await _context.SaveChangesAsync();
            return RedirectToAction("SignIn", "User");
        }
    }
}
