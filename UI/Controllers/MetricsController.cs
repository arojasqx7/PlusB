using Domain.DAL;
using Domain.Entities;
using log4net;
using UI.toastr;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MetricsController : Controller
    {
        ILog logger = LogManager.GetLogger(typeof(MetricsController));
        private PlusBContext db = new PlusBContext();
        private static int idPerfEval;

        public ActionResult PerformanceEvaluation()
        {
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => !x.FirstName.Contains("Unassigned")).OrderBy(x => x.FirstName), "ID", "FullName");
            return View();
        }

        [HttpPost]
        public ViewResult PerformanceEvaluation(int Id_Consultant, DateTime DateFrom, DateTime DateTo)
        {
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => !x.FirstName.Contains("Unassigned")).OrderBy(x => x.FirstName), "ID", "FullName");

            if (DateFrom > DateTo)
            {
                this.AddToastMessage("Date range validation","Date From is greather than Date To, please verify.", ToastType.Error);
                return View();
            }
            else
            {
                var performanceByRange = db.PerformanceEvalutions
                                        .Where(performance => performance.idConsultant == Id_Consultant
                                         && performance.Date >= DateFrom
                                         && performance.Date <= DateTo)
                                         .OrderBy(x => x.Date)
                                         .AsNoTracking()
                                         .ToList();

                return View(performanceByRange);
            }
        }

        public ActionResult viewDetails(int id)
        {
            ViewBag.getIncidentsList = GetIncidentsList();
            ViewBag.getIncidentsData = GetIncidentsSLAData();

            idPerfEval = id;
            PerformanceEvaluation perfEval = db.PerformanceEvalutions.Find(id);

            var technologyList = (from ticket in db.Tickets
                                  join performance in db.PerformanceEvalutions
                                  on ticket.Id_Consultant equals performance.idConsultant
                                  join tecnology in db.Technologies
                                  on ticket.Id_Technology equals tecnology.ID
                                  where ticket.AssignmentDate == perfEval.Date
                                  || ticket.ClosedDate == perfEval.Date
                                  && ticket.Id_Consultant == perfEval.idConsultant
                                  select tecnology)
                                  .AsNoTracking()
                                  .Distinct();

            var escalatedCount = (from performance in db.PerformanceEvalutions
                                  join ticket in db.Tickets
                                  on performance.idConsultant equals ticket.Id_Consultant
                                  where ticket.Status == "Escalated" &&
                                  ticket.EscalationDate == perfEval.Date &&
                                  ticket.Id_Consultant == perfEval.idConsultant
                                  select ticket)
                                  .AsNoTracking()
                                  .Count();

            ViewBag.Techdata = technologyList.ToList();
            ViewBag.EscalateData = escalatedCount;
            ViewBag.datePerf = perfEval.Date.Date;

            return View();
        }

        //Returns json with ticket to use in chart labels
        public JsonResult GetIncidentsList()
        {
                int urlID = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"]);

                if(urlID != 0)
                {
                    PerformanceEvaluation perfEval = db.PerformanceEvalutions.Find(urlID);

                    var incidentsList =  (from ticket in db.Tickets
                                         where ticket.Id_Consultant == perfEval.idConsultant
                                         && ticket.ClosedDate == perfEval.Date
                                         select "Ticket #" + " " + ticket.Id)
                                         .ToList();

                    return Json(incidentsList, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    PerformanceEvaluation perfEval = db.PerformanceEvalutions.Find(idPerfEval);

                    var incidentsList =  (from ticket in db.Tickets
                                         where ticket.Id_Consultant == perfEval.idConsultant
                                         && ticket.ClosedDate == perfEval.Date
                                         select "Ticket #" + " " + ticket.Id)
                                         .ToList();

                    return Json(incidentsList, JsonRequestBehavior.AllowGet);
                }
          }

        public JsonResult GetIncidentsSLAData()
        {
            int urlID = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"]);

            if (urlID != 0)
            {
                PerformanceEvaluation perfEval = db.PerformanceEvalutions.Find(urlID);

                var incidentsData = (from ticket in db.Tickets
                                    join customer in db.Customers
                                    on ticket.Id_Customer equals customer.Id
                                    join custSLA in db.CustomerSLAS
                                    on customer.Id equals custSLA.IdCustomer
                                    join sla in db.SLAS
                                    on custSLA.IdSLA equals sla.ID
                                    where ticket.Id_Consultant == perfEval.idConsultant
                                    && ticket.ClosedDate == perfEval.Date
                                    select 
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) > 0 &&
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 1 ? 95 :
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 1 &&
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 2 ? 84 :
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 2 &&
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 5 ? 76 :
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 5 &&
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 10 ? 57 :
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 10 &&
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 20 ? 39 :
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 20 &&
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 30 ? 26 :
                                                 (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) > 30 ? 7
                                                 : 0
                                    ).ToList(); ;

                return Json(incidentsData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                PerformanceEvaluation perfEval = db.PerformanceEvalutions.Find(idPerfEval);

                var incidentsData = (from ticket in db.Tickets
                                    join customer in db.Customers
                                    on ticket.Id_Customer equals customer.Id
                                    join custSLA in db.CustomerSLAS
                                    on customer.Id equals custSLA.IdCustomer
                                    join sla in db.SLAS
                                    on custSLA.IdSLA equals sla.ID
                                    where ticket.Id_Consultant == perfEval.idConsultant
                                    && ticket.ClosedDate == perfEval.Date
                                    select 
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) > 0 &&
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 1 ? 95 :
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 1 &&
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 2 ? 84 :
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 2 &&
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 5 ? 76 :
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 5 &&
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 10 ? 57 :
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 10 &&
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 20 ? 39 :
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) >= 20 &&
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) < 30 ? 26 :
                                                (ticket.TotalResolutionHours / sla.ResolutionTimeAverage) > 30 ? 7
                                                : 0
                                    ).ToList();

                return Json(incidentsData, JsonRequestBehavior.AllowGet);
            }
        }

        #region KPI Evalution Metrics Methods
        // This GET/POST open the KPI Evaluation Metric 
        public ActionResult KPIEvaluation()
        {
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => !x.FirstName.Contains("Unassigned")).OrderBy(x => x.FirstName), "ID", "FullName");
            return View();
        }

        [HttpPost]
        public ViewResult KPIEvaluation(int Id_Consultant, DateTime DateFrom, DateTime DateTo)
        {
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => !x.FirstName.Contains("Unassigned")).OrderBy(x => x.FirstName), "ID", "FullName");

            if (DateFrom > DateTo)
            {
                this.AddToastMessage("Date range validation", "Date From is greather than Date To, please verify.", ToastType.Error);
                return View();
            }
            else
            {
                var KPIByRange = db.KPIEvaluations.
                                 Where(kpi => kpi.idConsultant == Id_Consultant
                                 && kpi.Date >= DateFrom
                                 && kpi.Date <= DateTo)
                                 .OrderBy(x => x.Date)
                                 .AsNoTracking()
                                 .ToList();

                return View(KPIByRange);
            }
                
        }
        #endregion
    }
}
