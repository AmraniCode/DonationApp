using Microsoft.AspNetCore.Mvc;

namespace ChurchApp.Controllers
{
    public class AuthController : Controller
    {

        private readonly ApplicationDbContext _context;
        public AuthController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login(string message, int state)
        {

            try
            {
                if (AuthController.IsAuthenticated(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                    });
                }

                ViewBag.state = state;
                ViewBag.message = message;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        [HttpPost]
        public IActionResult Login(User model, string message, int state)
        {
            ViewBag.state = state;
            ViewBag.message = message;
            try
            {
                if (model == null)
                {
                    return RedirectToAction(nameof(Login), new
                    {
                        message = "You need to fill the username and password !!",
                        state = Helper.FAILD_STATE
                    }
                      );
                }

                var user_exist = _context.Users
                    .Where(e => e.Username.Equals(model.Username) && e.Password.Equals(model.Password))
                    .Include(e => e.IdProfils)
                    .Include(e => e.IdMemberNavigation)
                    .Include(e => e.IdChurchNavigation)
                    .FirstOrDefault();

                if (user_exist == null)
                {
                    return RedirectToAction(nameof(Login), new
                    {
                        message = "Wrong password or username",
                        state = Helper.FAILD_STATE
                    }
                       );
                }

                var profils = "";
                foreach (var profil in user_exist.IdProfils)
                {
                    profils += profil.Label + ";";
                }

                HttpContext.Session.SetInt32(Helper.ID_USER, (int)user_exist.IdUser);
                HttpContext.Session.SetInt32(Helper.ID_CHURCH, (int)user_exist.IdChurch);
                HttpContext.Session.SetInt32(Helper.ID_MEMBER, (int)user_exist.IdMember);
                HttpContext.Session.SetString(Helper.USER_NAME, user_exist.Username);
                HttpContext.Session.SetString(Helper.NAME, user_exist?.IdMemberNavigation.Name);
                HttpContext.Session.SetString(Helper.CHURCH_NAME, user_exist?.IdChurchNavigation.ChurchName);
                HttpContext.Session.SetString(Helper.SURNAME, user_exist.IdMemberNavigation.Surname);
                HttpContext.Session.SetString(Helper.PROFILS, profils);

                return RedirectToAction("Index", "Home", new
                {
                    message = "Login successful; Welcome " + user_exist.Username,
                    state = Helper.SUCCESS_STATE
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }



        [HttpPost]
        public IActionResult CreateChurch(Church church, User defaultAdmin, Member member)
        {
            try
            {
                if (church == null)
                {
                    ViewBag.member = member;
                    ViewBag.defaultAdmin = defaultAdmin;

                    ViewBag.message = "You need to enter default church information!!";
                    ViewBag.state = Helper.FAILD_STATE;
                    return View(church);
                }

                if (defaultAdmin == null)
                {
                    ViewBag.member = member;
                    ViewBag.defaultAdmin = defaultAdmin;

                    ViewBag.message = "You need to enter default admin information!!";
                    ViewBag.state = Helper.FAILD_STATE;
                    return View(church);
                }

                if (member == null)
                {
                    ViewBag.member = member;
                    ViewBag.defaultAdmin = defaultAdmin;

                    ViewBag.message = "You need to enter default member information!!";
                    ViewBag.state = Helper.FAILD_STATE;
                    return View(church);
                }

                _context.Churches.Add(church);
                member.IdChurchNavigation = church;
                _context.Members.Add(member);

                defaultAdmin.IdMemberNavigation = member;
                defaultAdmin.IdChurchNavigation = church;
                defaultAdmin.IsDefault = true;
                var profilMember = _context.Profils.Where(e => e.Label.Contains(Helper.ADMIN_PROFIL)).FirstOrDefault();
                defaultAdmin.IdProfils.Add(profilMember);
                _context.Users.Add(defaultAdmin);
                _context.SaveChanges();


                return RedirectToAction("Index", "Home", new
                {
                    message = "Church created successfuly; Welcom " + defaultAdmin.Username,
                    state = Helper.SUCCESS_STATE
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }
        public static bool IsAuthenticated(HttpContext context)
        {
            return !String.IsNullOrEmpty(context.Session.GetString(Helper.ID_USER));
        }


        public static bool IsAdmin(HttpContext context)
        {
            var profils = context.Session.GetString(Helper.PROFILS);

            return profils.Contains(Helper.ADMIN_PROFIL);
        }

        public static bool IsMember(HttpContext context)
        {
            var profils = context.Session.GetString(Helper.PROFILS);

            return profils.Contains(Helper.MEMBER_PROFIL);
        }



    }
}
