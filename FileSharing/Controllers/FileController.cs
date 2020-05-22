﻿using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileSharing.Controllers
{
    public class FileController : Controller
    {
        UserContext db = new UserContext();

        public const string INTERNAL_FILE_PATH = @"E:\Универ\8 семестр\Диплом\FileSharing\FileSharing\FileSharing\Content\Files\";
        public const string FILE_URL = "/Content/Files/";
        public const int FILE_FRAME_SIZE = 1048576; // 1024 * 1024;
        public const int FILE_ENSURANCE = 16;

        // GET: File
        public ActionResult Index()
        {
            //IEnumerable<File> files = db.Files.ToList();
            var files = db.Files.Include(f => f.User);

            return View(files);
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

                int? userId = null;
                System.IO.FileInfo file1 = new System.IO.FileInfo(INTERNAL_FILE_PATH + fileName);
                long size = file1.Length;
                File file = null;

                // добавляем файл в бд
                User user = null;
                if (User.Identity.IsAuthenticated)
                {
                    user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                    userId = user.Id;
                }
                file = db.Files.Add(new File { Name = fileName, SizeInBytes = size, UserId = userId, Date = DateTime.Now });
                if (user != null)
                {
                    user.Files.Add(file);
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int fileId)
        {
            File file = db.Files.FirstOrDefault(f => f.Id == fileId);
            string fileName = file.Name;
            string fullPath = Request.MapPath("~/Content/Files/" + fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            db.Files.Remove(file);
            db.SaveChanges();

            IEnumerable<File> files = db.Files.ToList();

            return View("Index", files);
        }

        public ActionResult Details(int id)
        {
            File file = db.Files.Include(f => f.User).FirstOrDefault(f => f.Id == id);

            string userName = User.Identity.Name;
            User user = db.Users.FirstOrDefault(u => u.Login == userName);

            if (user == null || file.User != user && !User.IsInRole("admin"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View(file);
        }

        public FilePathResult Download(int fileId)
        {
            File file = db.Files.FirstOrDefault(f => f.Id == fileId);
            // Имя файла (необязательно)
            string fileName = file.Name;
            //string fileName = "Test.html";
            // Путь к файлу
            string file_path = Server.MapPath("~/Content/Files/" + fileName);
            //string file_path = Server.MapPath("~/Content/Files/Test.html");
            // Тип файла - content-type
            string file_type = "text/html";
            return File(file_path, file_type, fileName);
        }
    }
}