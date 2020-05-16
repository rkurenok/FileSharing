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
    }
}