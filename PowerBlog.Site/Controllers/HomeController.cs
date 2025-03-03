using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerBlog.Site.Data;
using PowerBlog.Site.Models;
using PowerBlog.Site.Models.ViewModels;

namespace PowerBlog.Site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _context.Blogs.Where(b => b.IsPublish == true && b.Price == null).OrderByDescending(b => b.CreateDate).Take(4).ToListAsync();
            var categories = await _context.Categories.Take(5).ToListAsync();
            var viewModel = new HomeViewModel()
            {
                Blogs = blogs,
                Categories = categories
            };
            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
