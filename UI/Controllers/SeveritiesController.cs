using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;

namespace UI.Controllers
{
    public class SeveritiesController : Controller
    {
        private PlusBContext db = new PlusBContext();

        // GET: Severities
        public ActionResult Index()
        {
            return View(db.Severities.ToList());
        }

        // GET: Severities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Severity severity = db.Severities.Find(id);
            if (severity == null)
            {
                return HttpNotFound();
            }
            return View(severity);
        }

        // GET: Severities/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Severities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,SeverityName,SeverityNumber")] Severity severity)
        {
            if (ModelState.IsValid)
            {
                db.Severities.Add(severity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(severity);
        }

        // GET: Severities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Severity severity = db.Severities.Find(id);
            if (severity == null)
            {
                return HttpNotFound();
            }
            return View(severity);
        }

        // POST: Severities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,SeverityName,SeverityNumber")] Severity severity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(severity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(severity);
        }

        // GET: Severities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Severity severity = db.Severities.Find(id);
            if (severity == null)
            {
                return HttpNotFound();
            }
            return View(severity);
        }

        // POST: Severities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Severity severity = db.Severities.Find(id);
            db.Severities.Remove(severity);
            db.SaveChanges();
            return RedirectToAction("Index");
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
