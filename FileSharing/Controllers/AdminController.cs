using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Reflection;
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
        public ActionResult Index(string item, int page = 1)
        {
            //IEnumerable<User> users = db.Users;

            string property = "";
            //User user = db.Users.FirstOrDefault(u => u.Id == id);
            User user = new User();
            PropertyInfo[] properties = user.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                property += prop.Name + " ";
            }

            string[] masProperty = property.Split(' ');

            SelectList items = new SelectList(new List<string>(masProperty));
            ViewData["Items"] = items;

            //int pageSize = 3;
            //IEnumerable<User> usersPerPage = db.Users.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            int pageSize = 3;
            IEnumerable<User> usersPerPage = null;

            switch (item)
            {
                case "Id":
                    usersPerPage = db.Users.OrderBy(u => u.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Email":
                    usersPerPage = db.Users.OrderBy(u => u.Email).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Login":
                    usersPerPage = db.Users.OrderBy(u => u.Login).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Age":
                    usersPerPage = db.Users.OrderBy(u => u.Age).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Gender":
                    usersPerPage = db.Users.OrderBy(u => u.Gender).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "RoleId":
                    usersPerPage = db.Users.OrderBy(u => u.RoleId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    usersPerPage = db.Users.OrderBy(u => u.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Users.Count() };
            PageViewModel pvm = new PageViewModel { PageInfo = pageInfo, Users = usersPerPage };

            //ViewBag.UserPerPage = usersPerPage;
            //ViewData["pvm"] = pvm;

            return View(pvm);
        }

        //[HttpPost]
        //public ActionResult Index(string item, int page = 1)
        //{
        //    string property = "";
        //    //User user = db.Users.FirstOrDefault(u => u.Id == id);
        //    User user = new User();
        //    PropertyInfo[] properties = user.GetType().GetProperties();
        //    foreach (PropertyInfo prop in properties)
        //    {
        //        property += prop.Name + " ";
        //    }

        //    string[] masProperty = property.Split(' ');

        //    SelectList items = new SelectList(new List<string>(masProperty));
        //    ViewData["Items"] = items;

        //    int pageSize = 3;
        //    IEnumerable<User> usersPerPage = null;

        //    switch (item)
        //    {
        //        case "Id":
        //            usersPerPage = db.Users.OrderBy(u => u.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //        case "Email":
        //            usersPerPage = db.Users.OrderBy(u => u.Email).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //        case "Login":
        //            usersPerPage = db.Users.OrderBy(u => u.Login).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //        case "Age":
        //            usersPerPage = db.Users.OrderBy(u => u.Age).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //        case "Gender":
        //            usersPerPage = db.Users.OrderBy(u => u.Gender).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //        case "RoleId":
        //            usersPerPage = db.Users.OrderBy(u => u.RoleId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //        default:
        //            usersPerPage = db.Users.OrderBy(u => u.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //            break;
        //    }

        //    //int pageSize = 3;
        //    //IEnumerable<User> usersPerPage = db.Users.OrderBy(u => u.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
        //    PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Users.Count() };
        //    PageViewModel pvm = new PageViewModel { PageInfo = pageInfo, Users = usersPerPage };

        //    return View(pvm);
        //}


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

        public string Filter(int id)
        {
            string property = "";
            string result = "";
            User user = db.Users.FirstOrDefault(u => u.Id == id);
            PropertyInfo[] properties = user.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                property += prop.Name + " ";
            }

            string[] masProperty = property.Split(' ');

            SelectList items = new SelectList(new List<string>(masProperty));
            return result;
        }
    }
}