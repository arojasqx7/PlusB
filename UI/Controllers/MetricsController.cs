using Domain.DAL;
using Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class MetricsController : Controller
    {
        private PlusBContext db = new PlusBContext();
        private static int idPerfEval;

        [Authorize(Roles ="Administrator")]
        public ActionResult PerformanceEvaluation()
        {
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => !x.FirstName.Contains("Unassigned")).OrderBy(x => x.FirstName), "ID", "FullName");
            return View();
        }

        [HttpPost]
        public ViewResult PerformanceEvaluation(int Id_Consultant, DateTime DateFrom, DateTime DateTo)
        {
            
            var performanceByRange = db.PerformanceEvalutions.Where(x => x.idConsultant == Id_Consultant 
                                     && x.Date >= DateFrom && x.Date <= DateTo)
                                     .OrderBy(x=>x.Date)
                                    .ToList();

            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => !x.FirstName.Contains("Unassigned")).OrderBy(x => x.FirstName), "ID", "FullName");
            return View(performanceByRange);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult viewDetails(int id)
        {
            idPerfEval = id;
            PerformanceEvaluation perfEval = db.PerformanceEvalutions.Find(id);

            var technologyList = (from x in db.Tickets
                                 join y in db.PerformanceEvalutions
                                 on x.Id_Consultant equals y.idConsultant
                                 join z in db.Technologies
                                 on x.Id_Technology equals z.ID
                                 where x.AssignmentDate == perfEval.Date && x.Id_Consultant == perfEval.idConsultant
                                 select z);

            var escalatedCount = (from x in db.PerformanceEvalutions
                                  join y in db.Tickets
                                  on x.idConsultant equals y.Id_Consultant
                                  where y.Status == "Escalated" && 
                                  y.EscalationDate == perfEval.Date &&
                                  y.Id_Consultant == perfEval.idConsultant
                                  select y).Count();


            ViewBag.Techdata = technologyList.ToList();
            ViewBag.EscalateData = escalatedCount;
            ViewBag.datePerf = perfEval.Date.Date;
            return View();
        }

        public ContentResult GetIncidentsList()
        {
            PerformanceEvaluation perfEval = db.PerformanceEvalutions.Find(idPerfEval);

            var incidentsList = (from x in db.PerformanceEvalutions
                                 join y in db.Tickets
                                 on x.idConsultant equals y.Id_Consultant
                                 where y.Id_Consultant == perfEval.idConsultant &&
                                 y.ClosedDate == perfEval.Date
                                 select y.Id);

            ViewBag.IncidentList = incidentsList.ToList();

            return Content(JsonConvert.SerializeObject(incidentsList));
        }
    }
}