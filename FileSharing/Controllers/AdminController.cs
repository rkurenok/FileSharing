using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FileSharing.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        UserContext db = new UserContext();
        // GET: Admin
        public ActionResult Index()
        {
            IEnumerable<User> users = db.Users;

            return View(users);
        }


        public ActionResult DetailsAccount(int userId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == userId);

            return View(user);
        }

        public ActionResult DeleteAccount(int userId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == userId);

            db.Users.Remove(user);
            db.SaveChanges();

            IEnumerable<User> users = db.Users;

            return View("Index", users);
        }

        public ActionResult EditUserRole(int userId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == userId);

            ViewBag.Login = user.Login;
            ViewBag.RoleId = user.RoleId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserRole(EditRole model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            User user = db.Users.FirstOrDefault(u => u.Login == model.Login);
            user.RoleId = model.Role;
            db.SaveChanges();
            user = db.Users.Where(u => u.Login == model.Login && u.RoleId == model.Role).FirstOrDefault();

            if (user != null)
            {
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}