using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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

        public ActionResult EditAccount()
        {
            User user = null;

            using (UserContext db = new UserContext())
            {
                var userName = User.Identity.Name;
                user = db.Users.FirstOrDefault(u => u.Login == userName);
                ViewBag.Email = user.Email;
                ViewBag.Login = user.Login;
                ViewBag.Age = user.Age;
                ViewBag.Gender = user.Gender;
            }
                return View();
        }

        [HttpPost]
        public ActionResult EditAccount(EditAccount model)
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
                ViewBag.Email = user.Email;

                user.Email = model.Email;
                user.Login = model.Login;
                user.Age = model.Age;
                user.Gender = model.Gender;
                db.SaveChanges();

                FormsAuthentication.SetAuthCookie(model.Login, true);

                user = db.Users.Where(u => u.Email == model.Email && u.Login == model.Login && u.Age == model.Age && u.Gender == model.Gender).FirstOrDefault();

                if (user != null)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }
    }
}