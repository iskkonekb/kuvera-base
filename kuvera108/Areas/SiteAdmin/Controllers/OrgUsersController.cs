using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using DataTables.Mvc;
using Microsoft.AspNet.Identity;
using kuvera108.Areas.SiteAdmin.Models;
using kuvera108.Data;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity.Owin;
using kuvera108.Controllers;

namespace kuvera108.Areas.SiteAdmin.Controllers
{
    [Authorize]
    public class OrgUsersController : KuveraController
    {
        public OrgUsersController()
        {
        }

       public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, OrgUsersSearchAndCreateModel model)
        {
            IEnumerable<OrgUsersViewModel> query = SearchOrgUsers(requestModel, model, out int filteredCount);

            var totalCount = query.Count();

            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length);

            var data = query.Select(x => new
            {
                Id = x.Id,
                Org_Id = x.Org_Id,
                User_Id = x.User_Id,
                Owner_Id = x.Owner,
                OrgName = x.OrgName,
                OrgDescr = x.OrgDescr,
                User_Name = x.User_Name,
                User_Email = x.User_Email,
                User_FullName = x.User_FullName,
                Owner_Name = x.Owner_Name,
                Owner_Email = x.Owner_Email,
                Owner_FullName = x.Owner_FullName
            }).ToList();

            return Json(new DataTablesResponse
            (requestModel.Draw, data, filteredCount, totalCount),
                        JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<OrgUsersViewModel> SearchOrgUsers(IDataTablesRequest requestModel, OrgUsersSearchAndCreateModel model, out int filteredCount)
        {
            EFGenericRepository<OrgUsersViewModel> OrgUsers = new EFGenericRepository<OrgUsersViewModel>(DbContext);
            IEnumerable<OrgUsersViewModel> query = null;
            string Owner = User.Identity.GetUserId();
            if (model.Org_Id != null)
            {
                query = OrgUsers.GetWithInclude(x => x.Org_Id == model.Org_Id && x.Owner == Owner);
            }
            else
                query = OrgUsers.GetWithInclude(x => x.Owner == Owner);
            #region Filtering
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.User_FullName.Contains(value));
            }
            #endregion Filtering
            filteredCount = query.Count();
            #region Sorting
            // Sorting
            var sortedColumns = requestModel.Columns.GetSortedColumns();
            var orderByString = String.Empty;

            foreach (var column in sortedColumns)
            {
                orderByString += orderByString != String.Empty ? "," : "";
                orderByString += (column.Data) + (column.SortDirection == Column.OrderDirection.Ascendant ? " asc" : " desc");
            }
            query = query.OrderBy(orderByString == string.Empty ? "Name asc" : orderByString);
            #endregion Sorting
            return query;
        }

        // GET: OrgUsers/Create
        public ActionResult Create()
        {
            var model = new OrgUsersSearchAndCreateModel()
            {
                OrgList = GetOrgList(),
                Owner = User.Identity.GetUserId(),
                Id = Convert.ToString(Guid.NewGuid())
            };
            if (Request.IsAjaxRequest())
                return PartialView("_CreatePartial", model);
            return View(model);
        }

        // POST: OrgUsers/Create
        [HttpPost]
        public async Task<ActionResult> Create(OrgUsersSearchAndCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).ToString();

                ModelState.AddModelError("", "Не допустимое состояние модели! " + errors);
                model.OrgList = GetOrgList();
                return View("_CreatePartial", model);
            }
            var user = await UserManager.FindByEmailAsync(model.User_Email);
            if (user == null)
            {
                ModelState.AddModelError("User_Email", "Пользователь с электронной почтой " + model.User_Email + " не найден!");
                model.OrgList = GetOrgList();
                return View("_CreatePartial", model);
            }
            OrgUsers OrgUsers = new OrgUsers()
            {
                Id = model.Id,
                Org_Id = model.Org_Id,
                User_Id = user.Id,
                Owner = model.Owner
            };
            try
            {
                DbContext.OrgUsers.Add(OrgUsers);
                await DbContext.SaveChangesAsync();
                return Content("success");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка добавления пользователя в общество {ex.Message}");
                return View("_CreatePartial", model);
            }
        }

        public async Task<ActionResult> Details(string id)
        {
            EFGenericRepository<OrgUsersViewModel> query = new EFGenericRepository<OrgUsersViewModel>(DbContext);
            OrgUsersViewModel model = await query.GetAsync(x => x.Id == id);
            if (Request.IsAjaxRequest())
                return PartialView("_detailsPartial", model);
            return View(model);
        }

        // GET: OrgUsers/Delete
        public ActionResult Delete(string id)
        {
            EFGenericRepository<OrgUsersViewModel> query = new EFGenericRepository<OrgUsersViewModel>(DbContext);
            OrgUsersViewModel model = query.Get(x => x.Id == id);
            if (Request.IsAjaxRequest())
                return PartialView("_DeletePartial", model);
            return View(model);
        }

        // POST: OrgUsers/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteOrgUsers(string Id)
        {
            EFGenericRepository<OrgUsers> query = new EFGenericRepository<OrgUsers>(DbContext);
            OrgUsers model = await query.GetAsync(x => x.Id == Id);
            try
            {
                DbContext.OrgUsers.Attach(model);
                OrgUsers o = DbContext.OrgUsers.Remove(model);
                await DbContext.SaveChangesAsync();
                if (Request.IsAjaxRequest())
                {
                    return Content("success");
                }
                else
                    return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка обновления общества " + ex.Message);
                EFGenericRepository<OrgUsersViewModel> queryView = new EFGenericRepository<OrgUsersViewModel>(DbContext);
                OrgUsersViewModel modelView = await queryView.GetAsync(x => x.Id == Id);
                return View(Request.IsAjaxRequest() ? "_DeletePartial" : "Delete", modelView);
            }
        }

        [HttpGet]
        public ActionResult AdvancedSearch()
        {
            OrgUsersSearchAndCreateModel model = new OrgUsersSearchAndCreateModel()
            {
                OrgList = GetOrgList()
            };
            return View("_AdvancedSearchPartial", model);
        }

        private SelectList GetOrgList(object selectedValue = null)
        {
            string Owner = User.Identity.GetUserId();
            return new SelectList(DbContext.Orgs
                                            .Where(x => x.Owner == Owner)
                                            .Select(x => new { x.Id, x.Name }),
                                            "Id",
                                            "Name", selectedValue);
        }
    }
}