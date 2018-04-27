using Domain.DAL;
using Domain.Entities;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class DashboardController : Controller
    {
        private PlusBContext db = new PlusBContext();
        private ApplicationUserManager _userManager;
        ILog logger = LogManager.GetLogger(typeof(TicketAttachmentController));
        DateTime today = Convert.ToDateTime(DateTime.Today.ToShortDateString());

        [Authorize(Roles = "Administrator")]
        public ActionResult adminDashboard()
        {
            ViewBag.techBreakdownList = getTechnologyBreakdown();
            ViewBag.getHistoryLast7Days = getIncidentHistoryLast7Days();
            return View();
        }

        [Authorize(Roles ="Consultant")]
        public ActionResult consultantDashboard()
        {
            ViewBag.Data1 = totalCriticalConsultant().ToString();
            ViewBag.Data2 = totalMajorConsultant().ToString();
            ViewBag.Data3 = totalMinorConsultant().ToString();
            ViewBag.GeneralKBData = getGeneralKB().ToString();
            ViewBag.accountSettKBData = getAccountSettingsKB().ToString();
            ViewBag.configKBData = getConfigurationsKB().ToString();
            ViewBag.securityKBData = getSecurityKB().ToString();
            return View();
        }

        [Authorize(Roles = "Customer")]
        public ActionResult customerDashboard()
        {
            ViewBag.Data1 = totalCriticalCustomer().ToString();
            ViewBag.Data2 = totalMajorCustomer().ToString();
            ViewBag.Data3 = totalMinorCustomer().ToString();
            ViewBag.surveyAnsweredList = getSurveyHistoryLast7Days();
           // ViewBag.surveyAnsweredDays = getSurveyHistoryDays();
            return View();
        }

        #region Consultant Methods
        public int openTicketsConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(y => y.Consultant.Email.Equals(user.Email)
                          && y.Status == "Open")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int ticketsInBacklogConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(y => y.Consultant.Email == user.Email
                          && y.Status != "Closed")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int WIPticketsConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(y => y.Consultant.Email == user.Email
                          && y.Status == "Work In Progress")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int pendingTicketsConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(y => y.Consultant.Email == user.Email
                          && y.Status == "Pending Customer")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int solutionSuggestedTicketsConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(y => y.Consultant.Email == user.Email
                          && y.Status == "Solution Suggested")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int totalCriticalConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(y => y.Consultant.Email == user.Email
                          && y.Id_Severity == 1 
                          && y.Status != "Closed")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int totalMajorConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(y => y.Consultant.Email == user.Email
                          && y.Id_Severity == 2
                          && y.Status != "Closed")
                         .AsNoTracking()
                         .Count();
            return tickets;
        }
    
        public int totalMinorConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(y => y.Consultant.Email == user.Email
                          && y.Id_Severity == 3
                          && y.Status != "Closed")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int AssignedToday_Consultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(x => x.Consultant.Email == user.Email
                          && x.AssignmentDate == today)
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int ResolvedToday_Consultant()
        {          
            var user = UserManager.FindById(User.Identity.GetUserId());

            var tickets = db.Tickets.Where(x => x.Consultant.Email == user.Email
                          && x.ClosedDate == today)
                          .AsNoTracking()
                          .Count();             
            return tickets;
        }

        public decimal techAvgConsultant()
        {
            decimal tecnologyAvg;
            try
            { 
                var user = UserManager.FindById(User.Identity.GetUserId());

                var techAvg = db.Tickets.Where(x => x.Consultant.Email == user.Email && x.Status != "Closed")
                              .Select(x=>x.Technology.Weight)
                              .Sum();

                int backlog = ticketsInBacklogConsultant();
                tecnologyAvg = techAvg / backlog;
                return tecnologyAvg;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return tecnologyAvg = 0;
            }
        }

        public int getEscalated()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());

            var escalated =   db.Tickets.Where(ticket => ticket.Consultant.Email == user.Email
                              && ticket.Status == "Escalated")
                              .AsNoTracking()
                              .Count();
            return escalated;
        }

        public int getGeneralKB()
        {
            int idConsultant = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).ConsultantID);

            var generalKB =  db.KnowledgeBases.Where(kb => kb.idConsultant == idConsultant
                             && kb.Category == "General")
                             .AsNoTracking()
                             .Count();
            return generalKB;
        }

        public int getAccountSettingsKB()
        {
            int idConsultant = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).ConsultantID);

            var accountSett = db.KnowledgeBases.Where(kb => kb.idConsultant == idConsultant
                            && kb.Category == "Account Settings")
                             .AsNoTracking()
                             .Count();
            return accountSett;
        }

        public int getConfigurationsKB()
        {
            int idConsultant = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).ConsultantID);

            var configKB =    db.KnowledgeBases.Where(kb => kb.idConsultant == idConsultant
                              && kb.Category == "Configurations")
                              .AsNoTracking()
                              .Count();
            return configKB;
        }

        public int getSecurityKB()
        {
            int idConsultant = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).ConsultantID);

            var securityKB =    db.KnowledgeBases.Where(kb => kb.idConsultant == idConsultant
                                && kb.Category == "Security")
                                .AsNoTracking()
                                .Count();
            return securityKB;
        }
        #endregion

        #region Admin Methods
        public int consultantNo()
        {
            var consultantNo = db.Consultants.Where(consultant => consultant.ID != 1)
                               .AsNoTracking()
                               .Count();
            return consultantNo;           
        }

        public int companiesNo()
        {
            var companiesNo = db.Customers.Where(company => company.Id != 1)
                               .AsNoTracking()
                               .Count();
            return companiesNo;
        }

        public int openTickets()
        {
            var openNo = db.Tickets.Where(ticket => ticket.Status == "Open")
                               .AsNoTracking()
                               .Count();
            return openNo;
        }

        public int unassignedTickets()
        {
            var unnasNo = db.Tickets.Where(ticket => ticket.Id_Consultant == 1)
                               .AsNoTracking()
                               .Count();
            return unnasNo;
        }

        public JsonResult getTechnologyBreakdown()
        {
            var techBreakdown = (from tickets in db.Tickets
                                 join technologies in db.Technologies
                                 on tickets.Id_Technology equals technologies.ID
                                 group new { tickets, technologies } by new { technologies.Name } into c
                                 select new
                                 {
                                     label=  c.Key.Name,
                                     value = c.Select(x => x.technologies.Name).Count()
                                 })
                                 .AsNoTracking()
                                 .Take(3)
                                 .ToList();

            return Json (techBreakdown, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getIncidentHistoryLast7Days()
        {
            DateTime _7DaysAgo = DateTime.Now.AddDays(-7);
            var short7Days = _7DaysAgo.Date;

            var incidentLastDays = (from tickets in db.Tickets
                                    where DbFunctions.TruncateTime(tickets.Date) >= short7Days
                                    group new { tickets } by new { tickets.Date } into b
                                    select new
                                    {
                                        y = b.Key.Date,
                                        a = b.Select(x => x.tickets.Id).Count(),
                                        b = b.Where(x => x.tickets.Status == "Closed" && x.tickets.Date >= short7Days).Select(x => x.tickets.Id).Count()
                                    })
                                    .AsNoTracking()
                                    .ToList();

            return Json (incidentLastDays, JsonRequestBehavior.AllowGet);
        }
        #endregion

        # region Customer Methods
        public int createdTodayCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var createdToday =    db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                                  && ticket.Date == today)
                                  .AsNoTracking()
                                  .Count();
            return createdToday;
        }

        public int resolvedTodayCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var resolvedToday =   db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                                  && ticket.ClosedDate == today)
                                  .AsNoTracking()
                                  .Count();
            return resolvedToday;
        }

        public int callbacksTodayCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var callbackToday =   db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                                  && ticket.Status == "Callback")
                                  .AsNoTracking()
                                  .Count();
            return callbackToday;
        }

        public int escalatedTodayCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var escalatedToday =   db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                                   && ticket.ClosedDate == today
                                   && ticket.Status == "Escalated") 
                                  .AsNoTracking()
                                  .Count();
            return escalatedToday;
        }

        public int openTicketsCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var openTickets =      db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                                   && ticket.Status == "Open")
                                   .AsNoTracking()
                                   .Count();
            return openTickets;
        }

        public int WIPTicketsCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var wipTickets = db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                             && ticket.Status == "Work In Progress")
                             .AsNoTracking()
                             .Count();
            return wipTickets;
        }

        public int PCTicketsCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var pcTickets =  db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                             && ticket.Status == "Pending Customer")
                             .AsNoTracking()
                             .Count();
            return pcTickets;
        }

        public int SSTicketsCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var ssTickets =   db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                              && ticket.Status == "Solution Suggested")
                             .AsNoTracking()
                             .Count();
            return ssTickets;
        }

        public int closedTicketsCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var closedTickets = db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                                && ticket.Status == "Closed")
                                .AsNoTracking()
                                .Count();
            return closedTickets;
        }

        public int totalCriticalCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var tickets = db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                          && ticket.Id_Severity == 1 
                          && ticket.Status != "Closed")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int totalMajorCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var tickets = db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                          && ticket.Id_Severity == 2
                          && ticket.Status != "Closed")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public int totalMinorCustomer()
        {
            int customerID = Convert.ToInt32(UserManager.FindById(User.Identity.GetUserId()).CustomerID);

            var tickets = db.Tickets.Where(ticket => ticket.Id_Customer == customerID
                          && ticket.Id_Severity == 3
                          && ticket.Status != "Closed")
                          .AsNoTracking()
                          .Count();
            return tickets;
        }

        public JsonResult getSurveyHistoryLast7Days()
        {
            DateTime _7DaysAgo = DateTime.Now.AddDays(-7);
            var short7Days = _7DaysAgo.Date;

            var surveysLastDays =   (from surveys in db.Surveys
                                    where surveys.DateSent >= short7Days
                                    group new { surveys } by new { surveys.DateSent } into b
                                    select new
                                    {
                                        y =     b.Key.DateSent,
                                        item1 = b.Where(x => x.surveys.IsAnswered == 1
                                                && x.surveys.DateSent >= short7Days)
                                                .Select(x => x.surveys.ID)
                                                .Count()
                                    })
                                    .AsNoTracking()
                                    .ToList();

            return Json(surveysLastDays, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region public ApplicationUserManager UserManager
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion
    }
}