using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using Persistence.Repositories;
using log4net;
using PagedList;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using UI.toastr;

namespace UI.Controllers
{
    public class SLAsController : Controller
    {
        private PlusBContext db = new PlusBContext();
        private ISLARepository slaRepo;
        ILog logger = LogManager.GetLogger(typeof(ImpactsController));

        public SLAsController()
        {
            this.slaRepo = new SLARepository(new PlusBContext());
        }

        // GET: SLAs
        [Authorize(Roles = "Administrator")]
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

            var SLA = slaRepo.GetSLAs();

            if (!String.IsNullOrEmpty(searchString))
            {
                SLA = SLA.Where(s => s.Name.Contains(searchString));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(SLA.OrderBy(x => x.Name).ToPagedList(pageNumber, pageSize));
        }

        // GET: SLAs/Create
        public PartialViewResult Create()
        {
            return PartialView();
        }

        // POST: SLAs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreationDate,Name,Scope,ResolutionTimeAverage,SupportType,PriorityName,ResponseTime")] SLA sLA)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    slaRepo.InsertSLA(sLA);
                    slaRepo.Save();
                    this.AddToastMessage("SLA", "SLA created successfully!", ToastType.Success);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
                        this.AddToastMessage("SLA", "SLA already exists, please verify.", ToastType.Error);
                    }
                    else
                    {
                        throw;
                    }
                }  
            }
            return RedirectToAction("Index");
        }

        // GET: SLAs/Edit/5
        public PartialViewResult Edit(int id)
        {
            SLA sla = slaRepo.GetSLAByID(id);
            return PartialView("PartialSLA/_editSLA", sla);
        }

        // POST: SLAs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreationDate,Name,Scope,ResolutionTimeAverage,SupportType,PriorityName,ResponseTime")] SLA sLA)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    slaRepo.UpdateSLA(sLA);
                    slaRepo.Save();
                    this.AddToastMessage("SLA", "SLA edited successfully", ToastType.Success);
                    return Json(new { success = true });
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
                        this.AddToastMessage("SLA", "SLA already exists, please verify.", ToastType.Error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("PartialSLA/_editSLA", sLA);
        }

        // GET: SLAs/Delete/5
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            SLA sla = slaRepo.GetSLAByID(id);
            return PartialView("PartialSLA/_deleteSLA", sla);
        }

        // POST: SLAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SLA sLA = slaRepo.GetSLAByID(id);
            slaRepo.DeleteSLA(id);
            slaRepo.Save();
            this.AddToastMessage("SLA", "SLA has been deleted.", ToastType.Success);
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
