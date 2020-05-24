using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace FileSharing.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        UserContext db = new UserContext();
        // GET: Manage
        public ActionResult Index()
        {
            User user = null;
            IEnumerable<File> files;

            //using (UserContext db = new UserContext())
            //{
            //    var userName = User.Identity.Name;
            //    user = db.Users.FirstOrDefault(u => u.Login == userName);
            //    files = db.Files.Include(f => f.User);
            //}

            var userName = User.Identity.Name;
            user = db.Users.FirstOrDefault(u => u.Login == userName);
            files = db.Files.Include(f => f.User).Where(f => f.UserId == user.Id);

            ViewBag.Files = files;

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
        [ValidateAntiForgeryToken]
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

        public ActionResult EditFile(int fileId)
        {
            File file = db.Files.FirstOrDefault(f => f.Id == fileId);
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            EditFile editFile = new EditFile { Id = file.Id, Name = file.Name, AccessId = file.AccessId };

            if (user == null || file.User != user)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Name = file.Name;
            ViewBag.AccessId = file.AccessId;

            return View(editFile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditFile(EditFile model, int fileId)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            File file = db.Files.FirstOrDefault(f => f.Id == fileId);
            file.Name = model.Name;
            file.AccessId = model.AccessId;
            db.SaveChanges();

            file = db.Files.Where(f => f.Name == model.Name && f.AccessId == model.AccessId).FirstOrDefault();

            if (file != null)
            {
                return RedirectToAction("Index");
            }
            
            return View(model);
        }
    }
}