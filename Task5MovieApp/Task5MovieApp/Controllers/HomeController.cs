using Microsoft.AspNetCore.Mvc;
using Task5MovieApp.Models;
using System.Diagnostics;

namespace Task5MovieApp.Controllers
{
    public class HomeController : Controller//DataController//Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            //ViewData["movies"] = from m in LibraryContext.Movies
            //                     select m;
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