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
   // [Authorize(Roles ="Customer")]
    public class SurveysController : Controller
    {
        private PlusBContext db = new PlusBContext();

        // GET: Surveys
        public ActionResult Index()
        {
            var surveys = db.Surveys.Include(s => s.Ticket);
            return View(surveys.ToList());
        }

        // GET: Surveys/Create
        public ActionResult Create()
        {
            ViewBag.idTicket = new SelectList(db.Tickets, "Id", "ShortDescription");
            return View();
        }

        // POST: Surveys/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,DateSent,Answer1,Answer2,Answer3,Comments,DateAnswered,idTicket")] Survey survey)
        {
            if (ModelState.IsValid)
            {
                db.Surveys.Add(survey);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.idTicket = new SelectList(db.Tickets, "Id", "ShortDescription", survey.idTicket);
            return View(survey);
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
