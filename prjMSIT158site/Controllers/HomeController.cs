using Microsoft.AspNetCore.Mvc;
using prjMSIT158site.Models;
using System.Diagnostics;

namespace prjMSIT158site.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MyDBContext _context;

        public HomeController(ILogger<HomeController> logger, MyDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var show = _context.Categories;
            return View(show);
        }


        public IActionResult JSONTest()
        {
            return View();
        }

        public IActionResult First()
        {
            return View();
        }

        public IActionResult Address()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Spots()
        {
            return View();
        }

        public IActionResult CallAPI()
        {
            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        public IActionResult AutoComplete()
        {
            return View();
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