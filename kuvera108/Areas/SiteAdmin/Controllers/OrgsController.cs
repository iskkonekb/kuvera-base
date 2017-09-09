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
using System.Net;
using System.Data.Entity;
using kuvera108.Controllers;

namespace kuvera108.Areas.SiteAdmin.Controllers
{
    [Authorize]
    public class OrgsController : KuveraController
    {
         public OrgsController()
        {

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Get([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, Org model)
        {
            IEnumerable<Org> query = SearchOrgs(requestModel, model, out int filteredCount);

            var totalCount = query.Count();

            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length);

            var data = query.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Descr = x.Descr
            }).ToList();

            return Json(new DataTablesResponse
            (requestModel.Draw, data, filteredCount, totalCount),
                        JsonRequestBehavior.AllowGet);
        }

        private IEnumerable<Org> SearchOrgs(IDataTablesRequest requestModel, Org model, out int filteredCount)
        {
            EFGenericRepository<Org> Orgs = new EFGenericRepository<Org>(DbContext);
            IEnumerable<Org> query = null;
            string Owner = User.Identity.GetUserId();
            if (model.Name == null)
                query = Orgs.GetWithInclude(x=> x.Owner == Owner);
            else
                query = Orgs.GetWithInclude(x => x.Name.Contains(model.Name) && x.Owner == Owner);
            #region Filtering
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.Name.Contains(value));
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

        // GET: Orgs/Create
        public ActionResult Create()
        {
            var model = new Org()
            {
                Id = Convert.ToString(Guid.NewGuid()),
                Owner = User.Identity.GetUserId()
            };
            if (Request.IsAjaxRequest())
                return PartialView("_CreatePartial", model);
            return View(model);
        }

        // POST: Orgs/Create
        [HttpPost]
        public async Task<ActionResult> Create(Org model)
        {
            if (!ModelState.IsValid)
                return View("_OrgAdd", model);
            try
            {
                DbContext.Orgs.Add(model);
                await DbContext.SaveChangesAsync();
                return Content("success");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Ошибка добавления общества " + ex.Message);
                return View("_CreatePartial", model);
            }
        }

        // GET: Org/Edit
        public ActionResult Edit(string id)
        {
            EFGenericRepository<Org> Orgs = new EFGenericRepository<Org>(DbContext);
            Org model = new Org();
            IEnumerable<Org> query = Orgs.GetWithInclude(x => x.Id == id);
            foreach (var item in query)
            {
                model = item;
                break;
            }
            if (Request.IsAjaxRequest())
                return PartialView("_EditPartial", model);
            return View(model);
        }

        // POST: Org/Edit
        [HttpPost]
        public async Task<ActionResult> Edit(Org model)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View(Request.IsAjaxRequest() ? "_EditPartial" : "Edit", model);
            }
            EFGenericRepository<Org> Orgs = new EFGenericRepository<Org>(DbContext);
            if (model.Id == null)
            {
                ModelState.AddModelError("", "Не найдено общество для обновления");
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View(Request.IsAjaxRequest() ? "_EditPartial" : "Edit", model);
            }
            try
            {
                DbContext.Orgs.Attach(model);
                DbContext.Entry(model).State = EntityState.Modified;
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
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View(Request.IsAjaxRequest() ? "_EditPartial" : "Edit", model);
            }
        }

        public async Task<ActionResult> Details(string id)
        {
            EFGenericRepository<Org> Orgs = new EFGenericRepository<Org>(DbContext);
            Org model = await Orgs.GetAsync(x => x.Id == id);
            if (Request.IsAjaxRequest())
                return PartialView("_detailsPartial", model);
            return View(model);
        }

        // GET: Org/Delete
        public ActionResult Delete(string id)
        {
            EFGenericRepository<Org> Orgs = new EFGenericRepository<Org>(DbContext);
            Org model = Orgs.Get(x => x.Id == id);
           if (Request.IsAjaxRequest())
                return PartialView("_DeletePartial", model);
            return View(model);
        }

        // POST: Org/Delete
        [HttpPost, ActionName("Delete")]
        public async Task<ActionResult> DeleteOrg(string Id)
        {
            EFGenericRepository<Org> Orgs = new EFGenericRepository<Org>(DbContext);
            Org org = await Orgs.GetAsync(x => x.Id == Id);
            try
            {
                DbContext.Orgs.Attach(org);
                Org o = DbContext.Orgs.Remove(org);
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
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return View(Request.IsAjaxRequest() ? "_DeletePartial" : "Delete", org);
            }
        }

        [HttpGet]
        public ActionResult AdvancedSearch()
        {
            Org model = new Org();
            return View("_AdvancedSearchPartial", model);
        }
    }
}