using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.Entities;
using Persistence.Repositories;
using Domain.DAL;
using System.Data;

namespace UI.Controllers
{
    public class SeveritiesController : Controller
    {
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
                severityRepo.InsertSeverity(severity);
                severityRepo.Save();
                return RedirectToAction("Index");
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
            try { 
                if (ModelState.IsValid)
                {
                    severityRepo.UpdateSeverity(severity);
                    severityRepo.Save();
                    return Json(new { success = true });
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again.");
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
