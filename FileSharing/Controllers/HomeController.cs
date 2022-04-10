﻿using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileSharing.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string result = "Вы не авторизованы";

            if (User.Identity.IsAuthenticated)
            {
                result = "Ваш логин: " + User.Identity.Name;
            }

            ViewBag.Result = result;

            return View();
        }

        [Authorize(Roles = "admin")]
        public string About()
        {
            ViewBag.Message = "Your application description page.";

            //using (UserContext db = new UserContext())
            //{
            //    User user1 = db.Users.FirstOrDefault(u => u.Id == user.Id);

            //    if (user1.RoleId != 2)
            //    {
            //        RedirectToRoute("");
            //    }

            //}

            return "Это увидит только администратор";
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}