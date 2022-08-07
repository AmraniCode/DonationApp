using Microsoft.AspNetCore.Mvc;

namespace ChurchApp.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;
        public DonationController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult MyDonations()
        {
            try
            {
                if (!AuthController.IsAuthenticated(HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!AuthController.IsMember(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }

                var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);
                var idMember = HttpContext.Session.GetInt32(Helper.ID_MEMBER);
                var listDonation = _context.Donations.Where(e => e.IdChurch == idChurch && e.IdMember == idMember)
                    .ToList();

                return View(listDonation);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult AddDonation(Donation model)
        {
            try
            {
                if (!AuthController.IsAuthenticated(HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!AuthController.IsMember(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }
                if (model == null)
                {
                    return RedirectToAction(nameof(MyDonations), new
                    {
                        message = "You need to enter donation information",
                        state = Helper.FAILD_STATE
                    });
                }

                model.DateDonation = DateTime.Now;
                model.IdChurch = (int)HttpContext.Session.GetInt32(Helper.ID_CHURCH);
                model.IdMember = (int)HttpContext.Session.GetInt32(Helper.ID_MEMBER);

                _context.Donations.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(MyDonations), new
                {
                    state = Helper.SUCCESS_STATE,
                    message = "Operation done successfuly"
                });


                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        public IActionResult EditDonation(Donation model)
        {
            try
            {
                if (!AuthController.IsAuthenticated(HttpContext))
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (!AuthController.IsMember(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }
                if (model == null)
                {
                    return RedirectToAction(nameof(MyDonations), new
                    {
                        message = "You need to enter donation information",
                        state = Helper.FAILD_STATE
                    });
                }

                var oldDonation = _context.Donations.Where(e => e.IdDonation == model.IdDonation).AsNoTracking().FirstOrDefault();
                model.DateDonation = oldDonation.DateDonation;
                model.IdChurch = (int)HttpContext.Session.GetInt32(Helper.ID_CHURCH);
                model.IdMember = (int)HttpContext.Session.GetInt32(Helper.ID_MEMBER);

                _context.Donations.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(MyDonations), new
                {
                    state = Helper.SUCCESS_STATE,
                    message = "Operation done successfuly"
                });


            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");

            }
        }

        public IActionResult DeleteDonation(int id)
        {
            try
            {
                if (!AuthController.IsAuthenticated(HttpContext))
                {
                    return RedirectToAction("Login", "Auth", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }

                if (!AuthController.IsMember(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }
                var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);
                var idMember = HttpContext.Session.GetInt32(Helper.ID_MEMBER);
                var doantion = _context.Donations.Where(e => e.IdMember == idMember && e.IdChurch == idChurch && e.IdDonation == id).FirstOrDefault();
                if (doantion == null)
                {
                    return RedirectToAction(nameof(MyDonations), new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }

                _context.Donations.Remove(doantion);
                _context.SaveChanges();
                return RedirectToAction(nameof(MyDonations), new
                {
                    state = Helper.SUCCESS_STATE,
                    message = "Operation done successfuly"
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
