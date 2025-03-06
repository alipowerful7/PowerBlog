using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;

namespace PowerBlog.Site.Areas.Admin.Controllers
{
    [AdminAuthorize]
    [Area("Admin")]
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OfferController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var offers = await _context.OfferPays.ToListAsync();
            return View(offers);
        }
        public IActionResult CreatWithAmount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostCreatWithAmount(OfferPay offerPay)
        {
            return View();
        }
    }
}
