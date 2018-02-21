using Domain.DAL;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Persistence.Repositories;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UI.Controllers
{
    [Authorize(Roles = "Consultant,Customer")]
    public class TicketActivityController : Controller
    {
        private static int IDTicket;
        private ITicketActivityRepository activityRepo;
        private ApplicationUserManager _userManager;

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
                return RedirectToAction("Assigned", "Tickets", new { id = IDTicket });
            }
            else
            {
                return RedirectToAction("incidentCreated", "Tickets", new { id = IDTicket });
            }
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