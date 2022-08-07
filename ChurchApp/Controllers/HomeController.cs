using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ChurchApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string message, int state)
        {
            ViewBag.message = message;
            ViewBag.state = state;
            try
            {
                if (!AuthController.IsAuthenticated(HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
