using FileSharing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FileSharing.Controllers
{
    public class FileController : Controller
    {
        // GET: File
        public ActionResult Index()
        {
            IEnumerable<File> files;
            using (UserContext db = new UserContext())
            {
                files = db.Files.ToList();
            }
            return View(files);
        }
    }
}