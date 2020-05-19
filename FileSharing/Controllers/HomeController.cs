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
        public const string INTERNAL_FILE_PATH = @"E:\Универ\8 семестр\Диплом\FileSharing\FileSharing\FileSharing\Content\Files\";
        public const string FILE_URL = "/Content/Files/";
        public const int FILE_FRAME_SIZE = 1048576; // 1024 * 1024;
        public const int FILE_ENSURANCE = 16;

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

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            string fileName = "";
            if (upload != null)
            {
                // получаем имя файла
                fileName = System.IO.Path.GetFileName(upload.FileName);
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(Server.MapPath("~/Content/Files/" + fileName));
            }
            int? userId = null;
            File file = null;
            System.IO.FileInfo file1 = new System.IO.FileInfo(INTERNAL_FILE_PATH + fileName);
            long size = file1.Length;

            // добавляем файл в бд
            using (UserContext db = new UserContext())
            {
                if (User.Identity.IsAuthenticated)
                {
                    User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                    userId = user.Id;
                }

                db.Files.Add(new File { Name = fileName, SizeInBytes = size, UserId = userId });
                db.SaveChanges();

                file = db.Files.Where(f => f.Name == fileName && f.SizeInBytes == size).FirstOrDefault();
            }

            return RedirectToAction("Index");
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