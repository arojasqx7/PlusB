using Domain.DAL;
using Domain.Entities;
using Persistence.Repositories;
using System;
using System.Linq;
using System.Web.Mvc;

namespace UI.Controllers
{
    [Authorize(Roles = "Consultant")]
    public class TicketActivityController : Controller
    {
        private static int IDTicket;
        private ITicketActivityRepository activityRepo;

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
            var ticketActivity = new TicketActivity
            {
                Date = date,
                Time = time,
                Activity = activity,
                idTicket = IDTicket
            };

            using (var context = new PlusBContext())
            {
                context.TicketActivities.Add(ticketActivity);
                context.SaveChanges();
            }
            return RedirectToAction("Assigned","Tickets",new { id= IDTicket});
        }
    }
}