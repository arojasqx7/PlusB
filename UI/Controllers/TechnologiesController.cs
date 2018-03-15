using System.Data;
using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using Persistence.Repositories;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using log4net;
using PagedList;
using System;
using UI.toastr;

namespace UI.Controllers
{
    public class TechnologiesController : Controller
    {
        private ITechnologyRepository technologyRepo;
        ILog logger = LogManager.GetLogger(typeof(ImpactsController));

        public TechnologiesController()
        {
            this.technologyRepo = new TechnologyRepository(new PlusBContext());
        }

        public TechnologiesController(ITechnologyRepository technologyRepo)
        {
            this.technologyRepo = technologyRepo;
        }

        // GET: Technologies
        [Authorize(Roles ="Administrator")]
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

            var technologies = technologyRepo.GetTechnologies();
            if (!String.IsNullOrEmpty(searchString))
            {
                technologies = technologies.Where(s => s.Name.Contains(searchString));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(technologies.OrderBy(x => x.Name).ToPagedList(pageNumber,pageSize));
        }

        // GET: Technologies/Create
        public PartialViewResult Create()
        {
            return PartialView();
        }

        [HttpPost, ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Description,Weight")] Technology technology)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    technologyRepo.InsertTechnology(technology);
                    technologyRepo.Save();
                    this.AddToastMessage("Technologies", "Technology created successfully!", ToastType.Success);
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
                        this.AddToastMessage("Technologies", "Technology already exists, please verify.", ToastType.Error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction("Index");
        }

        //GET: Technologies/Edit/5
        public ActionResult Edit(int id)
        {
            Technology technology = technologyRepo.GetTechnologyByID(id);
            return PartialView("PartialTechnologies/_editTechnology",technology);
        }

        // POST: Technologies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Weight")] Technology technology)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    technologyRepo.UpdateTechnology(technology);
                    technologyRepo.Save();
                    this.AddToastMessage("Technology", "Technology edited successfully", ToastType.Success);
                    return Json(new { success = true });
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
                        this.AddToastMessage("Technologies", "Technology already exists, please verify.", ToastType.Error);
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("PartialTechnologies/_editTechnology", technology);
        }

        // GET: Technologies/Delete/5
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Technology technology = technologyRepo.GetTechnologyByID(id);
            return PartialView("PartialTechnologies/_deleteTechnology", technology);
        }

        // POST: Technologies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "ID,Name,Description,Weight")] int id)
        {
            Technology technology = technologyRepo.GetTechnologyByID(id);
            technologyRepo.DeleteTechnology(id);
            technologyRepo.Save();
            this.AddToastMessage("Technologies", "Technology has been deleted.", ToastType.Success);
            return Json(new { success = true });
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                technologyRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
