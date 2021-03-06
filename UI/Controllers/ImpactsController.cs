﻿using System.Linq;
using System.Web.Mvc;
using Domain.Entities;
using Persistence.Repositories;
using Domain.DAL;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using log4net;
using UI.toastr;

namespace UI.Controllers
{
    public class ImpactsController : Controller
    {
        ILog logger = LogManager.GetLogger(typeof(ImpactsController));
        private IImpactRepository impactRepo;

        public ImpactsController()
        {
            this.impactRepo = new ImpactsRepository(new PlusBContext());
        }

        // GET: Impacts
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var impacts = from s in impactRepo.GetImpacts()
                             select s;
            return View(impacts.ToList());
        }

        // GET: Impacts/Create
        public PartialViewResult Create()
        {
            return PartialView("PartialImpacts/_createImpact");
        }

        // POST: Impacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ImpactName,ImpactNumber")] Impact impact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    impactRepo.InsertImpact(impact);
                    impactRepo.Save();
                    this.AddToastMessage("Impacts", "Impact created successfully!", ToastType.Success);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                        if (sqlException != null)
                        {
                            logger.Error(sqlExc.ToString());
                            this.AddToastMessage("Impacts", "This impact already exists, please verify.", ToastType.Error);
                        }
                        else
                        {
                            throw;
                        }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Impacts/Edit/5
        [Authorize(Roles = "Administrator")]
        public PartialViewResult Edit(int id)
        {
            Impact impact = impactRepo.GetImpactByID(id);
            return PartialView("PartialImpacts/_editImpact", impact);
        }

        // POST: Impacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ImpactName,ImpactNumber")] Impact impact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    impactRepo.UpdateImpact(impact);
                    impactRepo.Save();
                    this.AddToastMessage("Impacts", "Impact edited successfully!", ToastType.Success);
                    return Json(new { success = true });
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
                        this.AddToastMessage("Impacts", "This impact already exists, please verify.", ToastType.Error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("PartialImpacts/_editImpact", impact);
        }

        // GET: Impacts/Delete/5
        [Authorize(Roles = "Administrator")]
        public PartialViewResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Impact impact = impactRepo.GetImpactByID(id);
            return PartialView("PartialImpacts/_deleteImpact", impact);
        }

        // POST: Impacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "Id,ImpactName,ImpactNumber")] int id)
        {
            Impact impact = impactRepo.GetImpactByID(id);
            impactRepo.DeleteImpact(id);
            impactRepo.Save();
            this.AddToastMessage("Impacts", "Impact has been deleted.", ToastType.Success);
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                impactRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
