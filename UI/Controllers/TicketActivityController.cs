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
using System.Web;
using System.Web.Mvc;
using UI.toastr;

namespace UI.Controllers
{
    [Authorize]
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
                    this.AddToastMessage("Incident", "Activity added to the incident number " + ticketActivity.idTicket, ToastType.Success);
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
            var user = UserManager.FindById(User.Identity.GetUserId());
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("notifications@plusb.com", "PlusB Service Desk Notification");
            var subject = "Activity notification in incident No.  " +  IDTicket;
            var to = new EmailAddress(emailToSend, "Customer");
            var plainTextContent = "Incident Update";
            var htmlContent = "<!doctype html> " +
                                "<html>" +
                                  "<body style='background-color:#85A885;font-family:sans-serif;font-size:14px;-webkit-font-smoothing:antialiased;line-height:1.4;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%;'>" +
                                    "<br/>"+
                                    "<br/>"+
                                    "<table border='0' cellpadding='0'cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                     " <tr>"+
                                        "<td>&nbsp;</td>"+
                                        "<td style='display:block;margin:0 auto !important;max-width:580px;padding:10px;width:580px;'>" +
                                          "<div style='box-sizing:border-box;display: block;margin:0 auto;max-width:580px;padding:10px;'>" +
                                            "<table style='background:#ffffff;border-radius:3px;width:100%;border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                              "<tr>"+
                                                "<td style='box-sizing:border-box;padding:20px;'>" +
                                                  "<table border='0' cellpadding='0'cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                                    "<tr>"+
                                                      "<td style='font-family:sans-serif;font-size:14px;vertical-align:top;'>" +
                                                        "<p>Hi there,</p>"+
                                                        "<p>An action occurred in an incident where you are owner. This is the update just sent by: " + user.Email + "</p>"+
                                                        "<table border='0' cellpadding='0'cellspacing='0' class='btn btn-primary'>"+
                                                          "<tbody>" +
                                                            "<tr>"+
                                                              "<td align='left'>"+
                                                                "<table border='0' cellpadding='0'cellspacing='0'>"+
                                                               "   <tbody>"+
                                                              "      <tr>"+
                                                             "         <td style='text-align:center;'><a><strong>" + activityToSend + "</strong></a></td>" +
                                                            "        </tr>"+
                                                           "       </tbody>"+
                                                          "      </table>"+
                                                         "     </td>"+
                                                        "    </tr>"+
                                                       "   </tbody>"+
                                                      "  </table>"+
                                                     "   <br/>"+
                                                     "   <p style='font-size:12px;'>This is an automatic email sent by the PlusB Consulting Service Desk system.</p>"+
                                                    "    <p style='font-size:12px;'>Thanks for being part of our business!</p>" +
                                                   "   </td>"+
                                                  "  </tr>"+
                                                "  </table>"+
                                              "  </td>"+
                                             " </tr>"+
                                            "</table>"+
                                            "<br/>"+
                                            "<div style='clear:both;margin-top:10px;width:100%;'>" +
                                              "<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                                "<tr><br/>"+
                                                  "<td style='padding-bottom:10px;padding-top:10px;text-decoration:none;color:#000;font-size:12px;'>" +
                                                   "PlusB Consulting Escazú, San José Costa Rica" +
                                                   "Don't like these emails? <a>Unsubscribe</a>." +
                                                 " </td>"+
                                                "</tr>"+
                                                "<tr>"+
                                                  "<td style='padding-bottom:10px;padding-top:10px;text-decoration:none;color:#000;font-size:12px;'>" +
                                                    "Powered by <a>PlusB Consulting S.A</a>." +
                                                  "</td>"+
                                                "</tr>"+
                                              "</table>"+
                                            "</div"+
                                          "</div>"+
                                        "</td>"+
                                        "<td style='color:#000;font-size:12px;'>&nbsp;</td>" +
                                      "</tr>"+
                                    "</table>"+
                                 "</body>"+
                                "</html>";
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