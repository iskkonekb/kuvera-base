using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using kuvera108.Migrations;
using kuvera108.Filters;

namespace kuvera108.Controllers
{
    public class HomeController : KuveraController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public void Create()
        {
            using (DbContext)
            {
                ChangeDatabase cd = new ChangeDatabase();
                cd.Up();
                StartpdateDb.RunMigration(DbContext, cd);
            }
        }
        [HttpPost]
        [MultipleButton(Name = "action", Argument = "Publish1")]
        public ActionResult PressPublish1Button()
        {
            return RedirectToAction("test", "home");
        }
    }
}