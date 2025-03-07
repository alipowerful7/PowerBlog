using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Attributes;
using PowerBlog.Site.Data;

namespace PowerBlog.Site.Controllers
{
    [IsLoggedIn]
    public class OfferController : Controller
    {
        private readonly ApplicationDbContext _context;
        public OfferController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> CalculationOffer(string? offerWord, decimal? amount)
        {
            decimal? offerAmount = 0;
            if (offerWord == null)
            {
                TempData["ErrorMessage"] = "کد تخفیف وارد شده صحیح نمی باشد.";
                return RedirectToAction("Index", "Order");
            }
            var offer = await _context.OfferPays.FirstOrDefaultAsync(o => o.OfferWord == offerWord);
            if (offer == null || offer.IsDisable == true)
            {
                TempData["ErrorMessage"] = "کد تخفیف وارد شده صحیح نمی باشد.";
                return RedirectToAction("Index", "Order");
            }
            if (offer.OfferPercentage != null)
            {
                offerAmount = amount - (amount * offer.OfferPercentage / 100);
            }
            else if (offer.OfferAmount != null)
            {
                offerAmount = amount - offer.OfferAmount;
                if (offerAmount < 0)
                {
                    offerAmount = 0;
                }
            }
            TempData["OfferAmount"] = offerAmount;
            return RedirectToAction("Index", "Order");
        }
    }
}
