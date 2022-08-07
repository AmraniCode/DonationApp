using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ChurchApp.Controllers
{
    public class AdminController : Controller
    {

        private readonly ApplicationDbContext _context;
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Member Management
        /// </summary>
        /// <param name="message"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AddMember(string message, int state)
        {
            if (!AuthController.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Login", "Auth", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            if (!AuthController.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }


            ViewBag.message = message;
            ViewBag.state = state;

            return View();
        }

        [HttpPost]
        public IActionResult AddMember(Member model)
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

                if (!AuthController.IsAdmin(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }

                if (model == null)
                {
                    return RedirectToAction(nameof(AddMember), new
                    {
                        message = "You need to enter member information",
                        state = Helper.FAILD_STATE
                    });
                }

                model.IdChurch = (int)HttpContext.Session.GetInt32(Helper.ID_CHURCH);

                _context.Members.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(ViewMembers), new
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

        public IActionResult EditMember(int id, string message, int state)
        {
            if (!AuthController.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Login", "Auth", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            if (!AuthController.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            ViewBag.message = message;
            ViewBag.state = state;

            var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);
            var member = _context.Members.Where(e => e.IdMember == id && e.IdChurch == idChurch).FirstOrDefault();

            if (member == null)
            {
                return RedirectToAction(nameof(ViewMembers), new
                {
                    message = "Invalid Member id!!!",
                    state = Helper.FAILD_STATE
                });
            }
            return View(member);
        }


        [HttpPost]
        public IActionResult EditMember(Member model)
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

                if (!AuthController.IsAdmin(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }

                if (model == null)
                {
                    return RedirectToAction(nameof(AddMember), new
                    {
                        message = "You need to enter meme information",
                        state = Helper.FAILD_STATE
                    });
                }

                _context.Members.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(EditMember), new
                {
                    id = model.IdMember,
                    state = Helper.SUCCESS_STATE,
                    message = "Operation done successfuly"
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }



        public IActionResult ViewMembers(string message, int state)
        {
            if (!AuthController.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Login", "Auth", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            if (!AuthController.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            ViewBag.message = message;
            ViewBag.state = state;

            var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);
            var ListMembers = _context.Members
                .Include(e => e.Users)
                .Where(e => e.IdChurch == idChurch)
                .ToList();

            return View(ListMembers);
        }



        public IActionResult DeleteMember(int id)
        {
            if (!AuthController.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Login", "Auth", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            if (!AuthController.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }
            var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);
            var member = _context.Members.Where(e => e.IdMember == id && e.IdChurch == idChurch).FirstOrDefault();
            if (member == null)
            {
                return RedirectToAction(nameof(ViewMembers), new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            _context.Members.Remove(member);
            _context.SaveChanges();
            return RedirectToAction(nameof(ViewMembers), new
            {
                state = Helper.SUCCESS_STATE,
                message = "Operation done successfuly"
            });

        }

        /// <summary>
        /// User Management
        /// </summary>
        /// <param name="message"></param>
        /// <param name="state"></param>
        /// <returns></returns>

        [HttpGet]
        public IActionResult AddUser(string message, int state)
        {
            if (!AuthController.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Login", "Auth", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            if (!AuthController.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);

            ViewBag.IdProfil = _context.Profils
                .AsNoTracking()
                   .OrderBy(n => n.IdProfil).Select(n => new SelectListItem
                   {
                       Value = n.IdProfil.ToString(),
                       Text = n.Label
                   }).ToList();

            ViewBag.IdMember = _context.Members
                .AsNoTracking()
                   .OrderBy(n => n.IdMember).Select(n => new SelectListItem
                   {
                       Value = n.IdMember.ToString(),
                       Text = n.Name + "-" + n.Surname
                   }).ToList();

            ViewBag.message = message;
            ViewBag.state = state;

            return View();
        }

        [HttpPost]
        public IActionResult AddUser(User model, int IdProfil)
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

                if (!AuthController.IsAdmin(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }

                if (model == null)
                {
                    return RedirectToAction(nameof(ViewUsers), new
                    {
                        message = "You need to enter meme information",
                        state = Helper.FAILD_STATE
                    });
                }

                if (_context.Users.Where(e => e.Username == model.Username).Any())
                {
                    return RedirectToAction(nameof(AddUser), new
                    {
                        message = "This user is already exist !!",
                        state = Helper.FAILD_STATE
                    });
                }

                var profil = _context.Profils.Where(e => e.IdProfil == IdProfil).FirstOrDefault();

                model.IdChurch = (int)HttpContext.Session.GetInt32(Helper.ID_CHURCH);
                model.IdProfils.Add(profil);
                _context.Users.Add(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(ViewUsers), new
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

        public IActionResult EditUser(int id)
        {
            if (!AuthController.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Login", "Auth", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            if (!AuthController.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);
            var user = _context.Users
                .Include(e => e.IdProfils)
                .Where(e => e.IdUser == id && e.IdChurch == idChurch).FirstOrDefault();

            //ViewBag.IdMember = new SelectList(_context.Members.Where(e => e.IdChurch == idChurch).ToList(), "IdChurch", "Name");


            ViewBag.IdProfil = _context.Profils
                .AsNoTracking()
                   .OrderBy(n => n.IdProfil).Select(n => new SelectListItem
                   {
                       Value = n.IdProfil.ToString(),
                       Text = n.Label
                   }).ToList();



            if (user == null)
            {
                return RedirectToAction(nameof(ViewUsers), new
                {
                    message = "Invalid User id!!!",
                    state = Helper.FAILD_STATE
                });
            }

            var listprofilExist = new List<int>();
            foreach (var profil in user.IdProfils)
            {
                listprofilExist.Add((int)profil.IdProfil);
            }

            ViewBag.listprofilExist = listprofilExist;

            return View(user);
        }


        [HttpPost]
        public IActionResult EditUser(User model)
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

                if (!AuthController.IsAdmin(HttpContext))
                {
                    return RedirectToAction("Index", "Home", new
                    {
                        message = "Access denied!!",
                        state = Helper.FAILD_STATE
                    });
                }

                if (model == null)
                {
                    return RedirectToAction(nameof(AddUser), new
                    {
                        message = "You need to enter meme information",
                        state = Helper.FAILD_STATE
                    });
                }

                if (_context.Users.Where(e => e.Username == model.Username).Any())
                {
                    return RedirectToAction(nameof(EditUser), new
                    {
                        id = model.IdUser,
                        message = "This user is already exist !!",
                        state = Helper.FAILD_STATE
                    });
                }

                _context.Users.Update(model);
                _context.SaveChanges();
                return RedirectToAction(nameof(EditUser), new
                {
                    id = model.IdUser,
                    state = Helper.SUCCESS_STATE,
                    message = "Operation done successfuly"
                });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

        }



        public IActionResult ViewUsers(string message, int state)
        {
            if (!AuthController.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Login", "Auth", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            if (!AuthController.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);
            var ListMembers = _context.Users
                .Include(e => e.IdMemberNavigation)
                .Include(e => e.IdProfils)
                .Where(e => e.IdChurch == idChurch)
                .ToList();

            return View(ListMembers);
        }



        public IActionResult DeleteUser(int id)
        {
            if (!AuthController.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Login", "Auth", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            if (!AuthController.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);
            var user = _context.Users.Where(e => e.IdUser == id && e.IdChurch == idChurch).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction(nameof(ViewUsers), new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(ViewUsers), new
            {
                state = Helper.SUCCESS_STATE,
                message = "Operation done successfuly"
            });

        }



        public IActionResult UpdateProfils(int[] Profils, int IdUser)
        {
            if (!AuthController.IsAuthenticated(HttpContext))
            {
                return RedirectToAction("Login", "Auth", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            if (!AuthController.IsAdmin(HttpContext))
            {
                return RedirectToAction("Index", "Home", new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            var idChurch = HttpContext.Session.GetInt32(Helper.ID_CHURCH);
            var user = _context.Users
                .Include(e => e.IdProfils)
                .Where(e => e.IdUser == IdUser && e.IdChurch == idChurch).FirstOrDefault();

            if (user == null)
            {
                return RedirectToAction(nameof(ViewUsers), new
                {
                    message = "Access denied!!",
                    state = Helper.FAILD_STATE
                });
            }

            foreach (var profilId in Profils)
            {
                var profil = _context.Profils.Where(e => e.IdProfil == profilId).FirstOrDefault();
                if (profil != null && !user.IdProfils.Contains(profil))
                {
                    user.IdProfils.Add(profil);
                }
            }

            if (user.IsDefault && !user.IdProfils.Where(e => e.Label.Contains(Helper.ADMIN_PROFIL)).Any())
            {
                var profil = _context.Profils.Where(e => e.Label == Helper.ADMIN_PROFIL).FirstOrDefault();
                user.IdProfils.Add(profil);
                _context.SaveChanges();
                return RedirectToAction(nameof(ViewUsers), new
                {
                    state = Helper.FAILD_STATE,
                    message = "You can not unselect admin profil for default user of the church"
                });
            }

            _context.SaveChanges();

            return RedirectToAction(nameof(ViewUsers), new
            {
                state = Helper.SUCCESS_STATE,
                message = "Operation done successfuly"
            });

        }







    }
}
