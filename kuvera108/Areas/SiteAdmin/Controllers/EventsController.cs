using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Mvc;
using DataTables.Mvc;
using kuvera108.Data;
using kuvera108.Areas.SiteAdmin.Models;
using System.Collections.Generic;
using kuvera108.Controllers;

namespace kuvera108.Areas.SiteAdmin.Controllers
{
    [Authorize(Roles = "admin")]
    public class EventsController : KuveraController
    {
        public EventsController()
        {

        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Get([ModelBinder(typeof(DataTablesBinder))] IDataTablesRequest requestModel, EventsSearchBox searchViewModel)
        {
            IEnumerable<Log> query = SearchEvents(requestModel, searchViewModel, out int filteredCount);

            var totalCount = query.Count();

            // Paging
            query = query.Skip(requestModel.Start).Take(requestModel.Length);

            var data = query.Select(x => new
            {
                Login = x.Login,
                Ip = x.Ip,
                Url = x.Url,
                Dt = x.Dt,
                Desc = x.Desc,
                Typ = x.Typ
            }).ToList();

            return Json(new DataTablesResponse
            (requestModel.Draw, data, filteredCount, totalCount),
                        JsonRequestBehavior.AllowGet);
        }
        private IEnumerable<Log> SearchEvents(IDataTablesRequest requestModel, EventsSearchBox searchViewModel, out int filteredCount)
        {
            EFGenericRepository<Log> Lg = new EFGenericRepository<Log>(DbContext);
            DateTime dtF = searchViewModel.FromDate == null ? DateTime.Now.Date : DateTime.Parse(searchViewModel.FromDate);
            DateTime dtT = searchViewModel.ToDate == null ? DateTime.Now.Date.AddHours(24).AddMilliseconds(-1) : DateTime.ParseExact(searchViewModel.ToDate, "dd/MM/yyyy HH:mm:ss", null);
            IEnumerable<Log> query = Lg.GetWithInclude(x => x.Dt >= dtF && x.Dt <= dtT);
            #region Filtering
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.Typ.Contains(value));
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
            query = query.OrderBy(orderByString == string.Empty ? "Dt asc" : orderByString);
            #endregion Sorting
            return query;
        }

        [HttpGet]
        public ActionResult EventsSearch()
        {
            var searchBox = new EventsSearchBox();

            return View("_EventsSearch", searchBox);
        }
    }
}