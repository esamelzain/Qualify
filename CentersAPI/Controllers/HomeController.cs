using CentersAPI.Models.EFModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CentersAPI.Controllers
{
    public class HomeController : Controller
    {
        private Entities db = new Entities();
        public ActionResult Index()
        {
            return RedirectToAction("Index", "App/Default");
        }
        public ActionResult Share(int courseId)
        {
            var playStoreUrl = db.Settings.SingleOrDefault().googlePlayURL;
            return Redirect(playStoreUrl);
        }
    }
}
