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
        public ActionResult Index(FileMessageId? fileMessage, AccountMessageId? accountMessage, string name)
        {
            ViewBag.StatusMessage =
                accountMessage == AccountMessageId.RegisterAccount ? "Ваш профиль " + name + " успешно создан" 
                : fileMessage == FileMessageId.UploadFile ? "Файлы были успешно загружены" 
                : "";
            string result = "Вы не авторизованы";

            if (User.Identity.IsAuthenticated)
            {
                result = "Ваш логин: " + User.Identity.Name;
            }

            ViewBag.Result = result;

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