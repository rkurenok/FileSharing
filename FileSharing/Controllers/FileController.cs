using FileSharing.Models;
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
            }
            int? userId = null;
            System.IO.FileInfo file1 = new System.IO.FileInfo(INTERNAL_FILE_PATH + fileName);
            long size = file1.Length;
            File file = null;

            // добавляем файл в бд
            using (UserContext db = new UserContext())
            {
                User user = null;
                if (User.Identity.IsAuthenticated)
                {
                    user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                    userId = user.Id;
                }
                file = db.Files.Add(new File { Name = fileName, SizeInBytes = size, UserId = userId });
                if (user != null)
                {
                    user.Files.Add(file);
                }
                db.SaveChanges();

                //file = db.Files.Where(f => f.Name == fileName && f.SizeInBytes == size).FirstOrDefault();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int fileId)
        {
            File file = db.Files.FirstOrDefault(f => f.Id == fileId);
            db.Files.Remove(file);
            db.SaveChanges();

            IEnumerable<File> files = db.Files.ToList();

            return View("Index", files);
        }
    }
}