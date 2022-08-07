using Microsoft.AspNetCore.Mvc;

namespace ChurchApp.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult DonationReport()
        {
            try
            {
                if (!AuthController.IsAuthenticated(HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!AuthController.IsAdmin(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }


                return View();


            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
