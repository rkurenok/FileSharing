using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace FileSharing.Controllers
{
    public class FileController : Controller
    {
        UserContext db = new UserContext();

        //public const string INTERNAL_FILE_PATH = @"G:\Универ\8 семестр\Диплом\FileSharing\FileSharing\FileSharing\Content\Files\";
        public const string INTERNAL_FILE_PATH = @"d:\DZHosts\LocalUser\armani\www.filesharing.somee.com\Content\Files\";
        public const string FILE_URL = "/Content/Files/";
        public const int FILE_FRAME_SIZE = 1048576; // 1024 * 1024;
        public const int FILE_ENSURANCE = 16;

        // GET: File
        public ActionResult Index(int? category = 0, int page = 1)
        {
            ViewBag.FileAccess = db.FileAccesses;

            int pageSize = 10;
            IEnumerable<File> files;
            ViewBag.CategoryId = category;
            if (category == 0)
            {
                files = db.Files.OrderBy(f => f.Id).Include(f => f.User).Where(f => f.AccessId == 2);
                ViewBag.CategoryId = category;
                ViewBag.Files = files.Count();
            }
            else
            {
                files = db.Files.OrderBy(f => f.Id).Include(f => f.User).Where(f => f.AccessId == 2);
                ViewBag.Files = files.Count();
                files = db.Files.OrderBy(f => f.Id).Include(f => f.User).Where(f => f.AccessId == 2 && f.CategoryId == category);
                ViewBag.CategoryId = category;
            }

            IEnumerable<File> filesPerPage = files.OrderBy(f => f.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = files.Count() };
            PageViewModel pvm = new PageViewModel { PageInfo = pageInfo, Files = filesPerPage };

            foreach (File file in files)
            {
                FileRetentionPeriod fileRetentionPeriod = null;
                fileRetentionPeriod = db.FileRetentionPeriods.FirstOrDefault(f => f.Id == file.FileRententionPeriodId);
                DateTime creation = file.Date;
                if ((DateTime.Now - creation).TotalDays > fileRetentionPeriod.Value)
                {
                    return RedirectToAction("Delete", "File", new { fileId = file.Id });
                }
            }
            ViewBag.StatusMessage = "";
            ViewBag.Categories = db.Categories;

            return View(pvm);
        }

        [HttpPost]
        public ActionResult Index(string serachFile, int page = 0)
        {
            IEnumerable<File> files = db.Files.OrderBy(f => f.Id).Include(f => f.User).Where(f => f.AccessId == 2);
            if (!string.IsNullOrEmpty(serachFile))
            {
                files = db.Files.Where(f => f.AccessId == 2 && f.Name.Contains(serachFile)).OrderBy(f => f.Id);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            if (files == null)
            {
                files = db.Files.OrderBy(f => f.Id).Include(f => f.User).Where(f => f.AccessId == 2);
                ViewBag.StatusMessage = "Совпадения не найдены";
            }
            ViewBag.Categories = db.Categories;
            int pageSize = 10;
            IEnumerable<File> filesPerPage = files.OrderBy(f => f.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = files.Count() };
            PageViewModel pvm = new PageViewModel { PageInfo = pageInfo, Files = filesPerPage };
            return View(pvm);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase[] uploads, string access, int retentionPeriodId)
        {
            string fileName = "";
            object routeValues = null;
            if (uploads[0] != null)
            {
                File file = null;
                foreach (HttpPostedFileBase uploadFile in uploads)
                {
                    // получаем имя файла
                    fileName = System.IO.Path.GetFileName(uploadFile.FileName);
                    // сохраняем файл в папку Files в проекте
                    uploadFile.SaveAs(Server.MapPath("~/Content/Files/" + fileName));

                    int? userId = null;
                    System.IO.FileInfo file1 = new System.IO.FileInfo(INTERNAL_FILE_PATH + fileName);
                    long size = file1.Length;

                    // добавляем файл в бд
                    User user = null;
                    if (User.Identity.IsAuthenticated)
                    {
                        user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                        userId = user.Id;
                    }
                    file = db.Files.Add(new File { Name = fileName, OriginalName = fileName, SizeInBytes = size, UserId = userId, Date = DateTime.Now, FileRententionPeriodId = retentionPeriodId });
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

                    IEnumerable<Category> categories = db.Categories;
                    string extention = System.IO.Path.GetExtension(INTERNAL_FILE_PATH + fileName);
                    foreach (var category in categories)
                    {
                        string[] fileExtention = category.FilesExtension.Split(' ');
                        for (int i = 0; i < fileExtention.Length; i++)
                        {
                            if (fileExtention[i] == extention)
                            {
                                file.CategoryId = category.Id;
                                break;
                            }
                        }
                    }
                    if (file.CategoryId == 0)
                    {
                        file.CategoryId = 8;
                    }
                }
                db.SaveChanges();
                routeValues = new { fileMessage = FileMessageId.UploadFile, fileId = file.Id };
            }
            return RedirectToAction("Index", "Home", routeValues);
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
            object routeValues = null;
            if (User.Identity.IsAuthenticated)
            {
                User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
                if (file.UserId == user.Id)
                {
                    routeValues = new { fileMessage = FileMessageId.DeleteFile, fileName };
                }
            }
            FileUniqueKey fileUniqueKey =  db.FileUniqueKeys.FirstOrDefault(k => k.FileId == file.Id);
            if (fileUniqueKey != null)
            {
                db.FileUniqueKeys.Remove(fileUniqueKey);
            }
            User user1 = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            if (user1.Id != file.UserId)
            {
                db.Files.Remove(file);
                db.SaveChanges();
                IEnumerable<File> files = db.Files.ToList();
                routeValues = new { fileMessage = FileMessageId.DeleteFile, fileName };
                return RedirectToAction("FileList", "Admin", routeValues);
            }
            else
            {
                db.Files.Remove(file);
                db.SaveChanges();
                IEnumerable<File> files = db.Files.ToList();
                routeValues = new { fileMessage = FileMessageId.DeleteFile, fileName };
                return RedirectToAction("Index", "Manage", routeValues);
            }
        }

        public ActionResult Details(int id)
        {
            File file = db.Files.Include(f => f.User).FirstOrDefault(f => f.Id == id);

            string userName = User.Identity.Name;
            User user = db.Users.FirstOrDefault(u => u.Login == userName);
            bool admin = User.IsInRole("admin");

            FileRetentionPeriod fileRetentionPeriod = null;
            fileRetentionPeriod = db.FileRetentionPeriods.FirstOrDefault(f => f.Id == file.FileRententionPeriodId);
            DateTime creation = file.Date;
            if ((DateTime.Now - creation).TotalDays > fileRetentionPeriod.Value)
            {
                return RedirectToAction("Delete", "File", new { fileId = file.Id });
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

        public ActionResult DownloadByLink(int fileId, string uniqueKey)
        {
            File file = db.Files.FirstOrDefault(f => f.Id == fileId);
            FileUniqueKey fileUniqueKey = db.FileUniqueKeys.FirstOrDefault(k => k.UniqueKey == uniqueKey);
            User user = db.Users.FirstOrDefault(u => u.Id == file.UserId);
            ViewBag.User = user;
            if (file.FileUniqueKeyId == fileUniqueKey.Id)
            {
                return View(file);
            }

            return RedirectToAction("Index", "Home", new { fileMessage = FileMessageId.FileDownload });
        }

        public ActionResult GetFileDownloadLink(int fileId)
        {
            File file = db.Files.FirstOrDefault(f => f.Id == fileId);
            var url = "";
            if (file.FileUniqueKeyId == null)
            {
                Constant saltConstant = db.Constants.First();
                string salt = saltConstant.Value;
                string login = "Anonym";
                DateTime date = DateTime.Now;
                string hash = sha1(sha1(login + date + salt));

                FileUniqueKey uniqueKey = db.FileUniqueKeys.Add(new FileUniqueKey { UniqueKey = hash, FileId = fileId });
                db.SaveChanges();
                file.FileUniqueKeyId = uniqueKey.Id;
                db.SaveChanges();
                url = Request.ServerVariables["HTTP_REFERER"];
            }

            //if (url.Contains("https://localhost:44301/Manage"))
            if (url.Contains("http://filesharing.somee.com/Manage"))
            {
                return RedirectToAction("Index", "Manage");
            }
            else
            {
                return RedirectToAction("Index", "Home", new { fileId = fileId });
            }
        }

        private string sha1(string input)
        {
            byte[] hash;
            using (var sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider())
                hash = sha1.ComputeHash(Encoding.Unicode.GetBytes(input));
            var sb = new StringBuilder();
            foreach (byte b in hash) sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }
    }

    public enum FileMessageId
    {
        DeleteFile,
        UploadFile,
        FileDownload,
        FileNotFound,
        Error
    }
}