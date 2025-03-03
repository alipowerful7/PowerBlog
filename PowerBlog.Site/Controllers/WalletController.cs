using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NuGet.DependencyResolver;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;
using System.Threading.Tasks;

namespace PowerBlog.Site.Controllers
{
    [IsLoggedIn]
    public class WalletController : Controller
    {
        private readonly ApplicationDbContext _context;
        public WalletController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == long.Parse(HttpContext.Session.GetString("UserId")));
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> PostPay(decimal? amount)
        {
            if (amount == null)
            {
                return NotFound();
            }
            amount = amount * 10;
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://gateway.zibal.ir/v1/request");
            var content = new StringContent("{\r\n    \"merchant\":\"zibal\",\r\n    \"amount\": " + amount + ",\r\n    \"callbackUrl\":\"https://localhost:44306/Wallet/GetPayStatus\"\r\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var jsonObj = JObject.Parse(result);
            var trackId = jsonObj["trackId"]?.ToString();
            amount = amount / 10;
            TempData["Amount"] = amount.ToString();
            return Redirect($"https://gateway.zibal.ir/start/{trackId}");
        }
        public async Task<IActionResult> GetPayStatus(string success, string status, string trackId)
        {
            if (int.Parse(success) != 1 || (int.Parse(status) != 1 && int.Parse(status) != 2))
            {
                TempData["ErrorMessage"] = "پرداخت ناموفق بود.";
                return RedirectToAction("Index", "Wallet");
            }
            var userId = long.Parse(HttpContext.Session.GetString("UserId"));
            var userWallet = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (userWallet == null)
            {
                return NotFound();
            }
            if (userWallet.Wallet == null)
            {
                userWallet.Wallet = decimal.Parse(TempData["Amount"].ToString());
            }
            else
            {
                userWallet.Wallet += decimal.Parse(TempData["Amount"].ToString());
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Wallet");
        }
        [HttpPost]
        public async Task<IActionResult> PostBuyBlog(decimal? amount)
        {
            if (amount == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == long.Parse(HttpContext.Session.GetString("UserId")));
            if (user == null)
            {
                return NotFound();
            }
            if ((user.Wallet ?? 0) < amount)
            {
                TempData["ErrorMessage"] = "موجودی کیف پول شما کمتر از مبلغ مورد نظر است.";
                return RedirectToAction("Index", "Order");
            }
            user.Wallet -= amount;
            var orders = await _context.Orders.Where(o => o.PaymentStatus == PaymentStatus.Pending && o.UserId == user.Id).ToListAsync();
            foreach (var order in orders)
            {
                order.PaymentStatus = PaymentStatus.Paid;
                order.PayDate = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Order");
        }
    }
}
