using System.Web;
using System.Web.Mvc;
using kuvera108.Areas.SiteAdmin.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using kuvera108.Data;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace kuvera108.Controllers
{
    [HandleError]
    public abstract class KuveraController : Controller
    {
        protected bool IsPostBack()
        {
            bool isPost = string.Compare(Request.HttpMethod, "POST",
               StringComparison.CurrentCultureIgnoreCase) == 0;
            if (Request.UrlReferrer == null) return false;

            bool isSameUrl = string.Compare(Request.Url.AbsolutePath,
               Request.UrlReferrer.AbsolutePath,
               StringComparison.CurrentCultureIgnoreCase) == 0;

            return isPost && isSameUrl;
        }
        private ApplicationDbContext _dbContext;
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        public ApplicationDbContext DbContext
        {
            get
            {
                return _dbContext ?? HttpContext.GetOwinContext().Get<ApplicationDbContext>();
            }
            private set
            {
                _dbContext = value;
            }

        }
        public KuveraController()
        {
        }
        public KuveraController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public KuveraController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public String ErrorMessage
        {
            get { return ViewBag.ErrorMessage ?? String.Empty; }
            set { ViewBag.ErrorMessage = value; }
        }
        public String ReturnUrl
        {
            get { return ViewBag.ReturnUrl ?? String.Empty; }
            set { ViewBag.ReturnUrl = value; }
        }
        private void FillOrgs()
        {
            string Owner = User.Identity.GetUserId();
            if (Owner == null) return;
            EFGenericRepository < OrgUsersViewModel > OrgUsers = new EFGenericRepository<OrgUsersViewModel>(DbContext);
            IEnumerable<OrgUsersViewModel> query = null;
                query = OrgUsers.GetWithInclude(x => x.User_Id == Owner);
            ViewBag.MyOrgList = query;
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpRequestBase Request = filterContext.HttpContext.Request;
            if (Request.HttpMethod == "GET")
            {
                FillOrgs();
                ReturnUrl = (Request.UrlReferrer != null) ? Request.UrlReferrer.ToString() : String.Empty;
            }
        }
        [HttpPost]
        public ActionResult FillOrgs(FormCollection fc)
        {
            return null;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }
    }
}