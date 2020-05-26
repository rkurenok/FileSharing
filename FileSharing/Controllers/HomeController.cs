using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileSharing.Controllers
{
    public class HomeController : Controller
    {
        UserContext db = new UserContext();
        public ActionResult Index(FileMessageId? fileMessage, AccountMessageId? accountMessage, string name, int? fileId)
        {
            ViewBag.StatusMessage =
                accountMessage == AccountMessageId.RegisterAccount ? "Ваш профиль " + name + " успешно создан" 
                : fileMessage == FileMessageId.UploadFile ? "Файлы были успешно загружены" 
                : fileMessage == FileMessageId.FileDownload ? "Файл больше недоступен или же никогда не существовал"
                : "";
            string result = "Вы не авторизованы";

            if (User.Identity.IsAuthenticated)
            {
                result = "Ваш логин: " + User.Identity.Name;
            }

            ViewBag.Result = result;

            File file = db.Files.FirstOrDefault(f => f.Id == fileId);
            ViewBag.File = file;

            IEnumerable<FileUniqueKey> uniqueKeys = db.FileUniqueKeys;
            ViewBag.FileUniqueKeys = uniqueKeys;

            return View();
        }

        public string About()
        {
            ViewBag.Message = "Your application description page.";

            return "Это увидит только администратор";
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}