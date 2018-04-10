using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using UI.toastr;
using log4net;

namespace UI.Controllers
{
    // [Authorize(Roles ="Customer")]
    public class SurveysController : Controller
    {
        ILog logger = LogManager.GetLogger(typeof(SurveysController));
        private PlusBContext db = new PlusBContext();

        // GET: Surveys
        public ActionResult Index()
        {
            var surveys = db.Surveys.Include(s => s.Ticket);
            return View(surveys.ToList());
        }

        public string Encode(string idToEncode)
        {
            byte[] encoded = System.Text.Encoding.UTF8.GetBytes(idToEncode);
            return Convert.ToBase64String(encoded);
        }

        // GET: Surveys/Create
        //[Authorize(Roles ="Customer")]
        public ActionResult fillSurvey()
        {
            int urlID = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"]);

            int validateIfAnswered = db.Surveys
                            .Where(ticketId => ticketId.idTicket == urlID)
                            .Select(flag => flag.IsAnswered)
                            .FirstOrDefault();

            if (validateIfAnswered == 0)
            {
                return View();
            }
            else
            {
                return RedirectToAction("answeredSurvey");
            }

        }

        // POST: Surveys/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult fillSurvey(Survey survey)
        {
            //Get ticket details
            int urlID = Convert.ToInt32(Url.RequestContext.RouteData.Values["id"]);
            Ticket ticket = db.Tickets.Find(urlID);

                try
                {
                    DateTime? dateTicketClosed = ticket.ClosedDate + ticket.ClosedTime;

                    var surveyDetails = new Survey
                    {
                        DateSent = dateTicketClosed ?? DateTime.Now,
                        Comments = survey.Comments,
                        DateAnswered = DateTime.Now,
                        idTicket = ticket.Id,
                        Answer1 = survey.Answer1,
                        Answer2 = survey.Answer2,
                        Answer3 = survey.Answer3,
                        Answer4 = survey.Answer4,
                        IsAnswered = 1
                    };

                    using (var context = new PlusBContext())
                    {
                        context.Surveys.Add(surveyDetails);
                        context.SaveChanges();
                        this.AddToastMessage("Survey", "Survey created successfully.", ToastType.Success);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                    this.AddToastMessage("Survey", "There was a problem while creating survey, please verify.", ToastType.Error);
                }

                return RedirectToAction("surveyAlertCreation");
        }

        [HttpGet]
        public ActionResult answeredSurvey()
        {
            return View("answeredSurvey");
        }

        [HttpGet]
        public ActionResult surveyAlertCreation()
        {
            return View("surveyAlertCreation");
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
