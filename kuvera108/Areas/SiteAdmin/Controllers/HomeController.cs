using System.Web.Mvc;
using kuvera108.Controllers;

namespace kuvera108.Areas.SiteAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public class HomeController : KuveraController
    {
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult Org()
        {
            return View("OrgAdmin");
        }
        [Authorize]
        public ActionResult Events()
        {
            return View("Events");
        }
    }
}