using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileSharing.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        // GET: Manage
        public ActionResult Index()
        {
            User user = null;

            using (UserContext db = new UserContext())
            {
                var userName = User.Identity.Name;
                user = db.Users.FirstOrDefault(u => u.Login == userName);
            }

            return View(user);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePassword model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = null;

            using (UserContext db = new UserContext())
            {
                var userName = User.Identity.Name;
                user = db.Users.FirstOrDefault(u => u.Login == userName);

                if (user.Password == model.OldPassword)
                {
                    user.Password = model.NewPassword;
                    db.SaveChanges();

                    user = db.Users.Where(u => u.Login == userName && u.Password == model.NewPassword).FirstOrDefault();

                    if (user != null)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);
        }
    }
}