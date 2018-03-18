using Domain.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class MetricsController : Controller
    {
        private PlusBContext db = new PlusBContext(); 

        [Authorize(Roles ="Administrator")]
        public ActionResult PerformanceEvaluation()
        {
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => !x.FirstName.Contains("Unassigned")).OrderBy(x => x.FirstName), "ID", "FullName");
            return View();
        }

        [HttpPost]
        public ActionResult consultPerformance()
        {
            //Make validation about Date To greather than Date From 

            return RedirectToAction("PerformanceEvaluation");
        }
    }
}