using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using Persistence.Repositories;
using log4net;
using PagedList;
using UI.toastr;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class KPIsController : Controller
    {
        private PlusBContext db = new PlusBContext();
        private IKPIRepository kpiRepo;
        ILog logger = LogManager.GetLogger(typeof(KPIsController));

        public KPIsController()
        {
            this.kpiRepo = new KPIRepository(new PlusBContext());
        }

        // GET: KPIs
        public ViewResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var KPI = kpiRepo.GetKPIs();

            if (!String.IsNullOrEmpty(searchString))
            {
                KPI = KPI.Where(s => s.Name.Contains(searchString));
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(KPI.OrderBy(x => x.Name).ToPagedList(pageNumber, pageSize));
        }

        // POST: KPIs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreationDate,Name,Objective,FormulaValue")] KPI kPI)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    kpiRepo.InsertKPI(kPI);
                    kpiRepo.Save();
                    this.AddToastMessage("KPI", "KPI created successfully!", ToastType.Success);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(ex.ToString());
                        this.AddToastMessage("KPI", "KPI already exists, please verify.", ToastType.Error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: KPIs/Edit/5
        public PartialViewResult Edit(int id)
        {
            KPI kPI = kpiRepo.GetKPIByID(id);
            return PartialView("PartialKPI/_editKPI", kPI);
        }

        // POST: KPIs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreationDate,Name,Objective,FormulaValue")] KPI kPI)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    kpiRepo.UpdateKPI(kPI);
                    kpiRepo.Save();
                    this.AddToastMessage("KPI", "KPI edited successfully", ToastType.Success);
                    return Json(new { success = true });
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
                        this.AddToastMessage("KPI", "KPI already exists, please verify.", ToastType.Error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("PartialKPI/_editKPI", kPI);
        }

        // GET: KPIs/Delete/5
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            KPI kpi = kpiRepo.GetKPIByID(id);
            return PartialView("PartialKPI/_deleteKPI", kpi);
        }

        // POST: KPIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KPI kPI = kpiRepo.GetKPIByID(id);
            kpiRepo.DeleteKPI(id);
            kpiRepo.Save();
            this.AddToastMessage("KPI", "KPI has been deleted.", ToastType.Success);
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
