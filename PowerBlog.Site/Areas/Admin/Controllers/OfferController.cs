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
            var offer = await _context.OfferPays.FirstOrDefaultAsync(x => x.OfferWord == offerPay.OfferWord);
            if (offer != null)
            {
                TempData["ErrorMessage"] = "کد تخفیف تکراری است";
                return RedirectToAction("CreatWithAmount", "Offer", new { area = "Admin" });
            }
            offerPay.CreatDate = DateTime.Now;
            await _context.OfferPays.AddAsync(offerPay);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Offer", new { area = "Admin" });
        }
        public IActionResult CreatWithPercentage()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> PostCreatWithPercentage(OfferPay offerPay)
        {
            var offer = await _context.OfferPays.FirstOrDefaultAsync(x => x.OfferWord == offerPay.OfferWord);
            if (offer != null)
            {
                TempData["ErrorMessage"] = "کد تخفیف تکراری است";
                return RedirectToAction("CreatWithAmount", "Offer", new { area = "Admin" });
            }
            offerPay.CreatDate = DateTime.Now;
            await _context.OfferPays.AddAsync(offerPay);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Offer", new { area = "Admin" });
        }
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var offer = await _context.OfferPays.FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var offer = await _context.OfferPays.FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null)
            {
                return NotFound();
            }
            return View(offer);
        }
        [HttpPost]
        public async Task<IActionResult> PostEdit(long? id, OfferPay offerPay)
        {
            if (id == null)
            {
                return NotFound();
            }
            var offer = await _context.OfferPays.FirstOrDefaultAsync(o => o.OfferWord == offerPay.OfferWord);
            if (offer != null)
            {
                TempData["ErrorMessage"] = "کد تخفیف تکراری است";
                return RedirectToAction("Edit", "Offer", new { area = "Admin", id = id.Value });
            }
            offerPay.Id = id.Value;
            _context.OfferPays.Update(offerPay);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Offer", new { area = "Admin" });
        }
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var offer = await _context.OfferPays.FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null)
            {
                return NotFound();
            }
            _context.OfferPays.Remove(offer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Offer", new { area = "Admin" });
        }
        public async Task<IActionResult> EnableOffer(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var offer = await _context.OfferPays.FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null)
            {
                return NotFound();
            }
            offer.IsDisable = false;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Offer", new { area = "Admin" });
        }
        public async Task<IActionResult> DisableOffer(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var offer = await _context.OfferPays.FirstOrDefaultAsync(o => o.Id == id);
            if (offer == null)
            {
                return NotFound();
            }
            offer.IsDisable = true;
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Offer", new { area = "Admin" });
        }
    }
}
