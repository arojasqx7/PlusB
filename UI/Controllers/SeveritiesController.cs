using System.Linq;
using System.Web.Mvc;
using Domain.Entities;
using Persistence.Repositories;
using Domain.DAL;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using log4net;
using UI.toastr;

namespace UI.Controllers
{
    public class SeveritiesController : Controller
    {
        ILog logger = LogManager.GetLogger(typeof(ImpactsController));
        private ISeverityRepository severityRepo;

        public SeveritiesController()
        {
            this.severityRepo = new SeveritiesRepository(new PlusBContext());
        }

        // GET: Severities
        [Authorize(Roles ="Administrator")] 
        public ActionResult Index()
        {
            var severities =   from s in severityRepo.GetSeverities()
                               select s;
            return View(severities.ToList());
        }

        // GET: Severities/Create
        public PartialViewResult Create()
        {
            return PartialView("PartialSeverities/_createSeverity");
        }

        // POST: Severities/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "Id,SeverityName,SeverityNumber")] Severity severity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    severityRepo.InsertSeverity(severity);
                    severityRepo.Save();
                    this.AddToastMessage("Severities", "Severity created successfully!", ToastType.Success);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
                        this.AddToastMessage("Severities", "This severity already exists, please verify.", ToastType.Error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Severities/Edit/5
        [Authorize(Roles = "Administrator")]
        public PartialViewResult Edit(int id)
        {
            Severity severity = severityRepo.GetSeverityByID(id);
            return PartialView("PartialSeverities/_editSeverity",severity);
        }

        // POST: Severities/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SeverityName,SeverityNumber")] Severity severity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    severityRepo.UpdateSeverity(severity);
                    severityRepo.Save();
                    this.AddToastMessage("Severities", "Severity edited successfully!", ToastType.Success);
                    return Json(new { success = true });
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
                        this.AddToastMessage("Severities", "This severity already exists, please verify.", ToastType.Error);
                        ViewBag.Message = "Record already exists.";
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("PartialSeverities/_editSeverity", severity);
        }

        // GET: Severities/Delete/5
        [Authorize(Roles = "Administrator")]
        public PartialViewResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Severity severity = severityRepo.GetSeverityByID(id);
            return PartialView("PartialSeverities/_deleteSeverity",severity);
        }

        // POST: Severities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "Id,SeverityName,SeverityNumber")] int id)
        {
            Severity severity = severityRepo.GetSeverityByID(id);
            severityRepo.DeleteSeverity(id);
            severityRepo.Save();
            this.AddToastMessage("Severities", "Severity has been deleted.", ToastType.Success);
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                severityRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
