using Domain.DAL;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
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
            return View();
        }

        [Authorize(Roles ="Consultant")]
        public ActionResult consultantDashboard()
        {
            ViewBag.Data1 = totalCriticalConsultant().ToString();
            ViewBag.Data2 = totalMajorConsultant().ToString();
            ViewBag.Data3 = totalMinorConsultant().ToString();
            return View();
        }

        [Authorize(Roles = "Customer")]
        public ActionResult customerDashboard()
        {
            return View();
        }

        #region Incident Status Consultant

        public int openTicketsConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(y => y.Consultant.Email.Equals(user.Email))
                         .Where(z =>z.Status.Equals("Open")).Count();
            return tickets;
        }

        public int ticketsInBacklogConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(y => y.Consultant.Email.Equals(user.Email)
                          && !y.Status.Equals("Closed")).Count();
            return tickets;
        }

        public int WIPticketsConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(y => y.Consultant.Email.Equals(user.Email)
                          && y.Status.Equals("Work In Progress")).Count();
            return tickets;
        }

        public int pendingTicketsConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(y => y.Consultant.Email.Equals(user.Email)
                          && y.Status.Equals("Pending Customer")).Count();
            return tickets;
        }

        public int solutionSuggestedTicketsConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(y => y.Consultant.Email.Equals(user.Email)
                          && y.Status.Equals("Solution Suggested")).Count();
            return tickets;
        }
        #endregion

        #region Get backlog totals in Pie chart for consultant
        // Get total of critical tickets for consultant
        public int totalCriticalConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(y => y.Consultant.Email.Equals(user.Email))
                         .Where(z => z.Id_Severity.Equals(1))
                         .Where(w=> !w.Status.Equals("Closed")).Count();
            return tickets;
        }

        // Get total of major tickets for consultant
        public int totalMajorConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(y => y.Consultant.Email.Equals(user.Email))
                         .Where(z => z.Id_Severity.Equals(2))
                         .Where(w => !w.Status.Equals("Closed")).Count();
            return tickets;
        }

        // Get total of minor tickets for consultant
        public int totalMinorConsultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = db.Tickets.Where(y => y.Consultant.Email.Equals(user.Email))
                         .Where(z => z.Id_Severity.Equals(3))
                         .Where(w => !w.Status.Equals("Closed")).Count();
            return tickets;
        }

        #endregion

        #region Assigned today
        public int AssignedToday_Consultant()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = (from a in db.Tickets where (a.Consultant.Email.Equals(user.Email)
                           && a.AssignmentDate == today)
                           select a).Count();
            return tickets;
        }
        #endregion

        #region Resolved today
        public int ResolvedToday_Consultant()
        {          
            var user = UserManager.FindById(User.Identity.GetUserId());
            var tickets = (from a in db.Tickets
                           where (a.Consultant.Email.Equals(user.Email) && a.ClosedDate == today)
                           select a).Count();
            return tickets;
        }
        #endregion

        #region TechAverage_Consultant 
        public decimal techAvgConsultant()
        {
            decimal tecnologyAvg;
            try
            { 
                var user = UserManager.FindById(User.Identity.GetUserId());
                var techAvg = db.Tickets.Where(x => x.Consultant.Email.Equals(user.Email) && !x.Status.Equals("Closed"))
                              .Select(x=>x.Technology.Weight).Sum();
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