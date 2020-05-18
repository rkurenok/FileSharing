using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}