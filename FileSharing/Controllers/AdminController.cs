using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public ActionResult Index(string item, AdminMessageId? adminMessage, string userName, int page = 1)
        {
            ViewBag.StatusMessage =
                adminMessage == AdminMessageId.DeleteAccount ? "Пользователь " + userName + " был удален"
                : "";

            string property = "";
            User user = db.Users.FirstOrDefault(u => u.Login == User.Identity.Name);
            PropertyInfo[] properties = user.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                property += prop.Name + " ";
            }

            string[] masProperty = property.Split(' ');

            SelectList items = new SelectList(new List<string>(masProperty));
            ViewData["Items"] = items;
            ViewBag.Item = item;

            int pageSize = 10;
            IEnumerable<User> usersPerPage = db.Users.OrderBy(u => u.Id).Include(u => u.Files).Skip((page - 1) * pageSize).Take(pageSize).ToList();

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

            IEnumerable<File> files = db.Files.Include(f => f.User);

            ViewBag.Files = files;

            return View(pvm);
        }

        public ActionResult AccountDetails(AdminMessageId? adminMessage, string userName, int? userId, int page = 1)
        {
            ViewBag.StatusMessage =
                adminMessage == AdminMessageId.EditUserRole ? "Роль пользователя " + userName + " была изменена"
                : "";

            User user = db.Users.FirstOrDefault(u => u.Id == userId);

            IEnumerable<File> files;

            files = db.Files.Include(f => f.User).Where(f => f.UserId == user.Id);

            ViewBag.Files = files;

            int pageSize = 8;

            IEnumerable<File> filesPerPage = db.Files.OrderBy(f => f.Id).Include(f => f.User).Where(f => f.UserId == user.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = files.Count() };
            PageViewModel pvm = new PageViewModel { PageInfo = pageInfo, Files = filesPerPage };

            ViewBag.User = user;

            if (user.Login == User.Identity.Name)
            {
                return RedirectToAction("Index", "Manage");
            }

            return View(pvm);
        }

        public ActionResult DeleteAccount(int userId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == userId);
            db.Users.Remove(user);
            db.SaveChanges();

            IEnumerable<User> users = db.Users;

            return RedirectToAction("Index", "Admin", new {users, adminMessage = AdminMessageId.DeleteAccount, userName = user.Login});
        }

        public ActionResult EditUserRole(int userId)
        {
            User user = db.Users.FirstOrDefault(u => u.Id == userId);

            ViewBag.Login = user.Login;
            ViewBag.RoleId = user.RoleId;
            ViewBag.UserId = userId;

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
                return RedirectToAction("AccountDetails", new { adminMessage = AdminMessageId.EditUserRole, userName = user.Login, userId = user.Id });
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

        public ActionResult FileList(string item, FileMessageId? fileMessage, string fileName, int page = 1)
        {
            ViewBag.StatusMessage = fileMessage == FileMessageId.DeleteFile ? "Файл " + fileName + " был удален"
                : "";

            var users = db.Users;
            ViewBag.Users = users;

            IEnumerable<File> files = db.Files;
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

            string property = "";
            File file1 = files.FirstOrDefault();
            PropertyInfo[] properties = file1.GetType().GetProperties();
            foreach (PropertyInfo prop in properties)
            {
                property += prop.Name + " ";
            }

            string[] masProperty = property.Split(' ');

            string[] output = new string[8];
            int counter = 0;

            for (int i = 0; i < masProperty.Length; i++)
            {
                if (masProperty[i] == "FileUniqueKey") continue;
                if (masProperty[i] == "FileUniqueKeyId") continue;
                if (masProperty[i] == "FileRententionPeriodId") continue;
                if (masProperty[i] == "FileRetentionPeriod") continue;
                if (masProperty[i] == "OriginalName") continue;
                if (masProperty[i] == "User") continue;
                if (masProperty[i] == "Access") continue;
                if (masProperty[i] == "Category") continue;
                output[counter] = masProperty[i];
                counter++;
            }

            SelectList items = new SelectList(new List<string>(output));
            ViewData["Items"] = items;
            ViewBag.Item = item;

            int pageSize = 10;
            IEnumerable<File> filesPerPage = db.Files.OrderBy(f => f.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();

            switch (item)
            {
                case "Id":
                    filesPerPage = db.Files.OrderBy(f => f.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Name":
                    filesPerPage = db.Files.OrderBy(f => f.Name).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "SizeInBytes":
                    filesPerPage = db.Files.OrderBy(f => f.SizeInBytes).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Date":
                    filesPerPage = db.Files.OrderBy(f => f.Date).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "AccessId":
                    filesPerPage = db.Files.OrderBy(f => f.AccessId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Access":
                    filesPerPage = db.Files.OrderBy(f => f.AccessId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "UserId":
                    filesPerPage = db.Files.OrderBy(f => f.UserId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "User":
                    filesPerPage = db.Files.OrderBy(f => f.UserId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "CategoryId":
                    filesPerPage = db.Files.OrderBy(f => f.CategoryId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                case "Category":
                    filesPerPage = db.Files.OrderBy(f => f.CategoryId).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
                default:
                    filesPerPage = db.Files.OrderBy(f => f.Id).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                    break;
            }

            PageInfo pageInfo = new PageInfo { PageNumber = page, PageSize = pageSize, TotalItems = db.Files.Count() };
            PageViewModel pvm = new PageViewModel { PageInfo = pageInfo, Files = filesPerPage };

            return View(pvm);
        }
    }

    public enum AdminMessageId
    {
        EditUserRole,
        DeleteAccount,
        Error
    }
}