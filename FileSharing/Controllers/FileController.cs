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
        public ActionResult Index(int page = 1)
        {
            //IEnumerable<File> files = db.Files.ToList();
            var files = db.Files.Include(f => f.User).Where(f => f.AccessId == 2);
            ViewBag.FileAccess = db.FileAccesses;

            int pageSize = 3;
            IEnumerable<File> filesPerPage = db.Files.OrderBy(f => f.Id).Include(f => f.User).Where(f => f.AccessId == 2).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = files.Count() };
            PageViewModel pvm = new PageViewModel { PageInfo = pageInfo, Files = filesPerPage };

            return View(pvm);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase[] uploads, string access)
        {
            string fileName = "";
            if (uploads != null)
            {
                foreach (HttpPostedFileBase uploadFile in uploads)
                {
                    // получаем имя файла
                    fileName = System.IO.Path.GetFileName(uploadFile.FileName);
                    // сохраняем файл в папку Files в проекте
                    uploadFile.SaveAs(Server.MapPath("~/Content/Files/" + fileName));

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
                    file = db.Files.Add(new File { Name = fileName, OriginalName = fileName, SizeInBytes = size, UserId = userId, Date = DateTime.Now });
                    if (access == "private")
                    {
                        file.AccessId = 1;
                    }
                    else
                    {
                        file.AccessId = 2;
                    }
                    if (user != null)
                    {
                        user.Files.Add(file);
                    }
                    //db.SaveChanges();
                }
                db.SaveChanges();
                // получаем имя файла
                //fileName = System.IO.Path.GetFileName(upload.FileName);
                //// сохраняем файл в папку Files в проекте
                //upload.SaveAs(Server.MapPath("~/Content/Files/" + fileName));

                //int? userId = null;
                //System.IO.FileInfo file1 = new System.IO.FileInfo(INTERNAL_FILE_PATH + fileName);
                //long size = file1.Length;
                //File file = null;

                //// добавляем файл в бд
                //User user = null;
                //if (User.Identity.IsAuthenticated)
                //{
                //    user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                //    userId = user.Id;
                //}
                //file = db.Files.Add(new File { Name = fileName, OriginalName = fileName, SizeInBytes = size, UserId = userId, Date = DateTime.Now });
                //if (access == "private")
                //{
                //    file.AccessId = 1;
                //}
                //else
                //{
                //    file.AccessId = 2;
                //}
                //if (user != null)
                //{
                //    user.Files.Add(file);
                //}
                //db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Delete(int fileId, int? page)
        {
            File file = db.Files.FirstOrDefault(f => f.Id == fileId);
            string fileName = file.OriginalName;
            string fullPath = Request.MapPath("~/Content/Files/" + fileName);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
            db.Files.Remove(file);
            db.SaveChanges();

            IEnumerable<File> files = db.Files.ToList();

            return RedirectToAction("FileList", "Admin", files);
        }

        public ActionResult Details(int id)
        {
            File file = db.Files.Include(f => f.User).FirstOrDefault(f => f.Id == id);

            string userName = User.Identity.Name;
            User user = db.Users.FirstOrDefault(u => u.Login == userName);
            bool admin = User.IsInRole("admin");
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
            string fileName = file.OriginalName;
            // Путь к файлу
            string file_path = Server.MapPath("~/Content/Files/" + fileName);
            // Тип файла - content-type
            string file_type = "application/octet-stream";
            return File(file_path, file_type, fileName);
        }
    }
}