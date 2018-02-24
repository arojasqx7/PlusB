using Domain.DAL;
using Domain.Entities;
using log4net;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Persistence.Repositories;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    [Authorize(Roles = "Consultant,Customer")]
    public class TicketActivityController : Controller
    {
        private static int IDTicket;
        private static string emailToSend;
        private static string activityToSend;
        private PlusBContext db = new PlusBContext();
        private ITicketActivityRepository activityRepo;
        private ApplicationUserManager _userManager;
        ILog logger = LogManager.GetLogger(typeof(TicketActivityController));

        public TicketActivityController()
        {
            activityRepo = new TicketActivitiesRepository(new PlusBContext());
        }

        [HttpGet]
        public PartialViewResult addActivity(int id)
        {
            IDTicket = id;
            return PartialView("PartialTicketActivity/_addActivity");
        }

        [HttpGet]
        public PartialViewResult insertedActivity(int id)
        {
            var activities = from s in activityRepo.GetActivitiesByTicketId(id)
                             select s;
            return PartialView("PartialTicketActivity/_insertedActivity", activities.ToList());
        }

        [HttpPost]
        public ActionResult Create(DateTime date,TimeSpan time, string activity)
        {
            try
            { 
                var user = UserManager.FindById(User.Identity.GetUserId());
                var ticketActivity = new TicketActivity
                {
                    Date = date,
                    Time = time,
                    Activity = activity,
                    idTicket = IDTicket,
                    User = user.Email
                };
                
                using (var context = new PlusBContext())
                {
                    context.TicketActivities.Add(ticketActivity);
                    context.SaveChanges();
                }

                if (User.IsInRole("Consultant"))
                {
                    var customerEmail = db.Tickets.Where(x => x.Id.Equals(IDTicket)).Select(x => x.Creator).FirstOrDefault();
                    emailToSend = customerEmail;
                    activityToSend = activity;
                    SendEmail(); // send notification 
                }
                else
                {
                    var consultantEmail = db.Tickets.Where(x => x.Id.Equals(IDTicket)).Select(x=>x.Id_Consultant).FirstOrDefault();
                    var consultant_Email = db.Consultants.Where(y => y.ID.Equals(consultantEmail)).Select(y=>y.Email).FirstOrDefault();
                    emailToSend = consultant_Email;
                    activityToSend = activity;
                    SendEmail(); // send notification 
                }

            }
            catch(Exception ex)
            {
                logger.Error(ex.ToString());
            }
     
            if (User.IsInRole("Consultant"))
            {
                return RedirectToAction("Assigned", "Tickets", new { id = IDTicket });
            }
            else
            {
                return RedirectToAction("incidentCreated", "Tickets", new { id = IDTicket });
            }
        }

        private void SendEmail()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("notifications@plusb.com", "PlusB Service Desk Notification");
            var subject = "Update Incident #  " +  IDTicket;
            var to = new EmailAddress(emailToSend, "Customer");
            var plainTextContent = "Incident Update";
            var htmlContent = "<p>The following update was made to the ticket</p><br/>" +
                              activityToSend;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response =client.SendEmailAsync(msg);
        }

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