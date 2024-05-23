using Microsoft.AspNetCore.Mvc;
using prjMSIT158site.Models;

namespace prjMSIT158site.Controllers
{
    public class HomeWorkController : Controller
    {
        private readonly MyDBContext _context;
        public HomeWorkController(MyDBContext context)
        {
            _context = context;
        }

        public IActionResult Homework1()
        {
            return View();
        }

        public IActionResult Homework2()
        {
            return View(_context.Addresses);
        }

        public IActionResult Homework3()
        {
            return View();
        }

        public IActionResult Homework4()
        {
            return View();
        }

        public IActionResult Homework5() 
        {
            return View();
        }

        public IActionResult Homework6() 
        {
            return View();
        }
    }
}
