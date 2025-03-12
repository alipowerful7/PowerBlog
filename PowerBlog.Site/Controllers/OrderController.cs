using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;

namespace PowerBlog.Site.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        [IsLoggedIn]
        public async Task<IActionResult> Index()
        {
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);
            var orders = await _context.Orders.Include(o => o.Blog).ThenInclude(b => b!.Category).Include(o => o.Blog).ThenInclude(b => b!.User).Where(o => o.UserId == userId && o.PaymentStatus == PaymentStatus.Pending).ToListAsync();
            return View(orders);
        }
        [HttpPost]
        public async Task<IActionResult> PostAddNewOrder(Order order, long? blogId)
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (userId == null)
            {
                TempData["ErrorMessageAddToOrder"] = "برای اضافه کردن مقاله به سبد خرید باید ابتدا وارد سایت شوید.";
                return RedirectToAction("Details", "PriceBlog", new { id = order.BlogId });
            }
            order.UserId = long.Parse(userId);
            order.PaymentStatus = PaymentStatus.Pending;
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "PriceBlog", new { id = blogId });
        }
        [HttpPost]
        public async Task<IActionResult> PostDeleteOrder(long? orderId)
        {
            if (orderId == null)
            {
                return NotFound();
            }
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
            {
                return NotFound();
            }
            var blogId = order.BlogId;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "PriceBlog", new { id = blogId });
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
            var content = new StringContent("{\r\n    \"merchant\":\"zibal\",\r\n    \"amount\": " + amount + ",\r\n    \"callbackUrl\":\"https://localhost:44306/Order/GetPayStatus\"\r\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var jsonObj = JObject.Parse(result);
            var trackId = jsonObj["trackId"]?.ToString();
            return Redirect($"https://gateway.zibal.ir/start/{trackId}");
        }
        public async Task<IActionResult> GetPayStatus(string success, string status, string trackId)
        {
            if (int.Parse(success) != 1 || (int.Parse(status) != 1 && int.Parse(status) != 2))
            {
                TempData["ErrorMessage"] = "پرداخت ناموفق بود.";
                return RedirectToAction("Index", "Order");
            }
            var userId = long.Parse(HttpContext.Session.GetString("UserId")!);
            var orders = await _context.Orders.Where(o => o.PaymentStatus == PaymentStatus.Pending && o.UserId == userId).ToListAsync();
            foreach (var order in orders)
            {
                order.PaymentStatus = PaymentStatus.Paid;
                order.TransactionId = trackId;
                order.PayDate = DateTime.Now;
                if (TempData["OfferWord"] != null)
                {
                    order.OfferPayId = long.Parse(TempData["OfferWord"]?.ToString()!);
                    TempData.Keep("OfferWord");
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Order");
        }
        //public async Task<IActionResult> SidebarOrder()
        //{
        //    var userIdString = HttpContext.Session.GetString("UserId");
        //    if (userIdString == null)
        //    {
        //        return NotFound();
        //    }
        //    var userId = long.Parse(userIdString);
        //    var orders = await _context.Orders.Where(o => o.PaymentStatus == PaymentStatus.Pending && o.UserId == userId).ToListAsync();
        //    return Ok(orders);
        //}
    }
}
