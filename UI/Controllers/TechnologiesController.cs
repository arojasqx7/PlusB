﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using Persistence.Repositories;

namespace UI.Controllers
{
    public class TechnologiesController : Controller
    {
        private ITechnologyRepository technologyRepo;

        public TechnologiesController()
        {
            this.technologyRepo = new TechnologyRepository(new PlusBContext());
        }

        public TechnologiesController(ITechnologyRepository technologyRepo)
        {
            this.technologyRepo = technologyRepo;
        }

        // GET: Technologies
        public ActionResult Index()
        {
            var technologies = from s in technologyRepo.GetTechnologies()
                               select s;
            return View(technologies.ToList());
        }

        // GET: Technologies/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Technology technology = db.Technologies.Find(id);
        //    if (technology == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(technology);
        //}

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
                technologyRepo.InsertTechnology(technology);
                technologyRepo.Save();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        //GET: Technologies/Edit/5
        public ActionResult Edit(int id)
        {
            Technology technology = technologyRepo.GetTechnologyByID(id);
            return PartialView("_editTechnology",technology);
        }

        // POST: Technologies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Description,Weight")] Technology technology)
        {
            try { 
            if (ModelState.IsValid)
            {
                technologyRepo.UpdateTechnology(technology);
                technologyRepo.Save();
                return Json(new { success = true });
            }
            }
            catch (DataException)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. Try again.");
            }
            return PartialView("_editTechnology", technology);
        }

        // GET: Technologies/Delete/5
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Technology technology = technologyRepo.GetTechnologyByID(id);
            return PartialView("_deleteTechnology",technology);
        }

        // POST: Technologies/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "ID,Name,Description,Weight")] int id)
        {
            Technology technology = technologyRepo.GetTechnologyByID(id);
            technologyRepo.DeleteTechnology(id);
            technologyRepo.Save();
            return RedirectToAction("/Technologies/Index");
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