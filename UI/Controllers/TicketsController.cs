﻿using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using UI.Extensions;
using Persistence.Repositories;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using PagedList;
using log4net;
using UI.toastr;
using Microsoft.AspNet.Identity;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace UI.Controllers
{
    public class TicketsController : Controller
    {
        ILog logger = LogManager.GetLogger(typeof(TicketsController));
        private PlusBContext db = new PlusBContext();
        private ITicketsRepository ticketRepo;
        private static int IDTicket;
        private static string emailToSend;
        private static string statusToSend;
        private static double hoursResult;
        private static int idCustomerIndex;
        private ApplicationUserManager _userManager;
        private static double riskPercentage = 0;
        private static double riskPercentageSLAFailure = 0;
        private static double riskPeriodicUpdate = 0;

        public TicketsController()
        {
            this.ticketRepo = new TicketsRepository(new PlusBContext());
        }

        // GET: Tickets
        [Authorize(Roles ="Customer, Administrator")]
        public ViewResult Index(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            if (User.IsInRole("Administrator"))
            {
                idCustomerIndex = 1;
            }
            else
            {
                idCustomerIndex = int.Parse(User.Identity.GetCustomerId());
            }
            
            var tickets = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                          .Where(x=>x.Id_Customer.Equals(idCustomerIndex) && !x.Status.Equals("Closed"));

            if (!String.IsNullOrEmpty(searchString))
            {
                tickets = tickets.Where(s => s.ShortDescription.Contains(searchString));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(tickets.OrderBy(x => x.Severity.SeverityNumber).ThenBy(x => x.Date).ToPagedList(pageNumber, pageSize));
        }

        //Show Unassigned Tickets list 
        [Authorize(Roles = "Consultant, Administrator")]
        public ViewResult UnassignedList(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            var unassignedTickets = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                                    .Where(x => x.Consultant.FirstName.Equals("Unassigned"));

            if (!String.IsNullOrEmpty(searchString))
            {
                unassignedTickets = unassignedTickets.Where(s => s.ShortDescription.Contains(searchString));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(unassignedTickets.OrderBy(x => x.Severity.SeverityNumber).ThenBy(x => x.Date).ToPagedList(pageNumber, pageSize));
        }

        //Show List of assigned tickets (Logged consultant). 
        [Authorize(Roles = "Consultant")]
        public ViewResult MyTickets(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            int idConsultantLogged = int.Parse(User.Identity.GetConsultantId());

            var myTickets = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                                    .Where(x => x.Id_Consultant.Equals(idConsultantLogged) && !x.Status.Equals("Closed"));

            if (!String.IsNullOrEmpty(searchString))
            {
                myTickets = myTickets.Where(s => s.ShortDescription.Contains(searchString));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(myTickets.OrderBy(x => x.Severity.SeverityNumber).ThenBy(x => x.Date).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Consultant")]
        public ViewResult Resolved(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            int idConsultantLogged = int.Parse(User.Identity.GetConsultantId());
            var resolved = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                                    .Where(x => x.Id_Consultant.Equals(idConsultantLogged) && x.Status.Equals("Closed"));

            if (!String.IsNullOrEmpty(searchString))
            {
                resolved = resolved.Where(s => s.ShortDescription.Contains(searchString));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(resolved.OrderBy(x => x.Severity.SeverityNumber).ThenBy(x => x.Date).ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Customer")]
        public ViewResult resolvedIncidents(string currentFilter, string searchString, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;

            int idCustomer = int.Parse(User.Identity.GetCustomerId());
            var resolved = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                                    .Where(x => x.Id_Customer.Equals(idCustomer) && x.Status.Equals("Closed"));

            if (!String.IsNullOrEmpty(searchString))
            {
                resolved = resolved.Where(s => s.ShortDescription.Contains(searchString));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(resolved.OrderBy(x => x.Severity.SeverityNumber).ThenBy(x => x.Date).ToPagedList(pageNumber, pageSize));
        }

        // GET ticket resolved by Consultant for Admin 
        [Authorize(Roles = "Administrator")]
        public ActionResult resolvedIncidentsByConsultant()
        {
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => !x.FirstName.Contains("Unassigned")).OrderBy(x => x.FirstName), "ID", "FullName");
            return View();
        }

        //POST for Incidents by consultant
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ViewResult resolvedIncidentsByConsultant(int Id_Consultant, DateTime DateFrom, DateTime DateTo)
        {
            var ticketsResolvedbyRange =  ticketRepo.GetTickets()
                                         .Where(x=>x.ClosedDate >= DateFrom && x.ClosedDate <= DateTo && x.Id_Consultant.Equals(Id_Consultant))
                                         .OrderBy(x => x.ClosedDate).ThenBy(x=>x.ClosedTime)
                                         .ToList();

            ViewBag.TodalTickets = ticketsResolvedbyRange.Count();
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => !x.FirstName.Contains("Unassigned")).OrderBy(x => x.FirstName), "ID", "FullName");
            return View(ticketsResolvedbyRange);
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Administrator,Customer,Consultant")]
        public ActionResult Details(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return PartialView("PartialTickets/_detailsTicket", ticket);
        }

        //   get: tickets/edit/5
        [Authorize(Roles = "Customer, Administrator")]
        public PartialViewResult Edit(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            ViewBag.Id_Consultant = new SelectList(db.Consultants, "ID", "FirstName", ticket.Id_Consultant);
            ViewBag.Id_Customer = new SelectList(db.Customers, "Id", "CompanyName", ticket.Id_Customer);
            ViewBag.Id_Impact = new SelectList(db.Impacts, "Id", "ImpactName", ticket.Id_Impact);
            ViewBag.Id_Severity = new SelectList(db.Severities, "Id", "SeverityName", ticket.Id_Severity);
            ViewBag.Id_TaskType = new SelectList(db.TaskTypes, "Id", "TaskName", ticket.Id_TaskType);
            ViewBag.Id_Technology = new SelectList(db.Technologies, "ID", "Name", ticket.Id_Technology);
            return PartialView("PartialTickets/_editTicket", ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer, Administrator")]
        public ActionResult Edit([Bind(Include = "Id,Date,OpenTime,Id_Customer,ShortDescription,LongDescription,Environment,Id_Technology,Id_Severity,Id_Impact,Id_TaskType,Status,Id_Consultant")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id_Consultant = new SelectList(db.Consultants, "ID", "FirstName", ticket.Id_Consultant);
            ViewBag.Id_Customer = new SelectList(db.Customers, "Id", "CompanyName", ticket.Id_Customer);
            ViewBag.Id_Impact = new SelectList(db.Impacts, "Id", "ImpactName", ticket.Id_Impact);
            ViewBag.Id_Severity = new SelectList(db.Severities, "Id", "SeverityName", ticket.Id_Severity);
            ViewBag.Id_TaskType = new SelectList(db.TaskTypes, "Id", "TaskName", ticket.Id_TaskType);
            ViewBag.Id_Technology = new SelectList(db.Technologies, "ID", "Name", ticket.Id_Technology);
            return Json(new { success = true });
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Customer")]
        public PartialViewResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return PartialView("PartialTickets/_deleteTicket", ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "Id,Date,OpenTime,Id_Customer,ShortDescription,LongDescription,Environment,Id_Technology,Id_Severity,Id_Impact,Id_TaskType,Status,Id_Consultant,files")] int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            ticketRepo.DeleteTicket(id);
            ticketRepo.Save();
            return Json(new { success = true });
        }

        //code to assign ticket to consultant 
        [Authorize(Roles = "Consultant")]
        public ActionResult AssignTicket(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return PartialView("PartialTickets/_assignTicket", ticket);
        }

        [HttpPost]
        public ActionResult AssignTicket(Ticket objTicket)
        {
            DateTime today = DateTime.Now;
            TimeSpan currentHour = TimeSpan.Parse(today.ToString("HH:mm:ss tt"));
            var shortDate = today.Date;
            int idConsultant = int.Parse(User.Identity.GetConsultantId()); // obtain id from claims

            Ticket ticket = ticketRepo.GetTicketByID(objTicket.Id); // Get all ticket details
            ticket.Id_Consultant = idConsultant; //assigning consultantID to ticket
            ticket.AssignmentDate = shortDate;
            ticket.AssignmentTime = currentHour;
            ticketRepo.UpdateTicket(ticket);
            ticketRepo.Save();
            this.AddToastMessage("Incidents", "Incident # " + ticket.Id + " assigned to me!", ToastType.Success);
            return RedirectToAction("MyTickets", "Tickets");
        }

        // GET
        [Authorize(Roles = "Consultant")]
        public ActionResult Assigned(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return View(ticket);
        }

        [Authorize]
        public ActionResult incidentClosed(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return View(ticket);
        }

        // GET
        [Authorize(Roles = "Administrator, Customer")]
        public ActionResult incidentCreated(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return View(ticket);
        }

        /// <summary>
        /// This function calculates the Average Resolution percentage for the ticket which is being CLOSED. 
        /// </summary>

       public double obtainAvgResolution()
        {
            Ticket ticket = ticketRepo.GetTicketByID(IDTicket);
            double resolutionAvg =0;
            double severityWeight;
            double techWeight = ticket.Technology.Weight;
            DateTime  hourTicketOpened =  ticket.Date + ticket.OpenTime;
            DateTime?  hourTicketClosed =  ticket.ClosedDate + ticket.ClosedTime;
            
            //sustract times 
            TimeSpan hoursDiff = Convert.ToDateTime(hourTicketClosed).Subtract(hourTicketOpened);
            hoursResult = hoursDiff.TotalHours;

            switch (ticket.Severity.SeverityName)
            {
                case "Critical":
                    severityWeight = 90;
                    resolutionAvg = severityWeight + hoursResult + techWeight;
                    break;

                case "Major":
                    severityWeight = 70;
                    resolutionAvg = severityWeight + hoursResult + techWeight;
                    break;

                case "Minor":
                    severityWeight = 40;
                    resolutionAvg = severityWeight + hoursResult + techWeight;
                    break;
            }

            return resolutionAvg;
        }

        /// <summary>
        /// This method assign the closed date & time, Avg resolution 
        /// Also updates the ticket status and call SendMail() function. 
        /// </summary>

        [HttpPost]
        public ActionResult UpdateStatus(int id, string status)
        {
            DateTime today = DateTime.Now;
            TimeSpan currentCloseHour = TimeSpan.Parse(today.ToString("HH:mm:ss tt"));
            var shortDate = today.Date;
            IDTicket = id;
            Ticket ticket = ticketRepo.GetTicketByID(id);
            ticket.Status = status;

            if (status == "Closed")
            {
                try
                {
                    ticket.ClosedDate = shortDate;
                    ticket.ClosedTime = currentCloseHour;
                    ticket.AverageResolution = obtainAvgResolution();
                    ticket.TotalResolutionHours = hoursResult;
                    ticketRepo.UpdateTicket(ticket);
                    ticketRepo.Save();
                    this.AddToastMessage("Incidents", "Incident number " + ticket.Id + " updated to " + ticket.Status, ToastType.Success);
                    sendSurveyEmail();
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }
            }
            else
            {
                try
                {
                    ticket.ClosedDate = null;
                    ticketRepo.UpdateTicket(ticket);
                    ticketRepo.Save();
                    this.AddToastMessage("Incidents", "Incident number " + ticket.Id + " updated to " + ticket.Status, ToastType.Success);
                }
                catch (Exception ex)
                {
                    logger.Error(ex.ToString());
                }
            }
           
                //return to proper page according to role
                if (User.IsInRole("Consultant"))
                {
                    var customerEmail = db.Tickets.Where(x => x.Id.Equals(IDTicket)).Select(x => x.Creator).FirstOrDefault();
                    emailToSend = customerEmail;
                    statusToSend = status;
                    SendEmail(); // send notification
                    return RedirectToAction("MyTickets", "Tickets");
                }
                else
                {
                    var consultantEmail = db.Tickets.Where(x => x.Id.Equals(IDTicket)).Select(x => x.Id_Consultant).FirstOrDefault();
                    var consultant_Email = db.Consultants.Where(y => y.ID.Equals(consultantEmail)).Select(y => y.Email).FirstOrDefault();
                    emailToSend = consultant_Email;
                    statusToSend = status;
                    SendEmail(); // send notification
                    return RedirectToAction("Index", "Tickets");
                }
        }

        [HttpPost]
        public ActionResult EscalateTicket(int id, string escalateReason)
        {
            DateTime today = DateTime.Now;
            var shortDate = today.Date;

            try
            {
                Ticket ticket = ticketRepo.GetTicketByID(id);
                ticket.Status = "Escalated";
                ticket.EscalationReason = escalateReason;
                ticket.EscalationDate = shortDate;
                ticketRepo.UpdateTicket(ticket);
                ticketRepo.Save();
                this.AddToastMessage("Incidents", "Incident number " + ticket.Id + " was " + ticket.Status, ToastType.Success);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
            return RedirectToAction("Index", "Tickets");
        }

        // Risk Monitor Region
        #region Risk Calculation Methods
        public double escalationRisk(int id)
        {
            //Get ticket per id received from Ajax call
            Ticket ticket = ticketRepo.GetTicketByID(id);

            //Get open hour and current hour to perform the hours diff calculation
            DateTime hourTicketOpened = ticket.Date + ticket.OpenTime;
            DateTime currentHour = DateTime.Now;

            //Obtain hours value
            TimeSpan hoursDiff = Convert.ToDateTime(currentHour).Subtract(hourTicketOpened);
            hoursResult = hoursDiff.TotalHours;

            var getSLAType = from a in db.Tickets
                             join b in db.Customers
                             on a.Id_Customer equals b.Id
                             join c in db.CustomerSLAS
                             on b.Id equals c.IdCustomer
                             join d in db.SLAS
                             on c.IdSLA equals d.ID
                             where a.Id == id
                             select new { d.SupportType, d.ResolutionTimeAverage};

            string supportType = getSLAType.Select(x => x.SupportType).First();
            int responseAvg = Convert.ToInt32(getSLAType.Select(y => y.ResolutionTimeAverage).First());
            double escalationAVG = 0;

            switch (supportType)
            {
                case "Standard":
                    escalationAVG = hoursResult - responseAvg;

                    if (escalationAVG >= 2 && escalationAVG <= 5)
                    {
                        riskPercentage = 12.6;
                    }
                    else if (escalationAVG >= 5 && escalationAVG <= 9)
                    {
                        riskPercentage = 24.1;
                    }
                    else if (escalationAVG > 9 && escalationAVG <= 12)
                    {
                        riskPercentage = 47;
                    }
                    else if (escalationAVG > 12 && escalationAVG <= 15)
                    {
                        riskPercentage = 58;
                    }
                    else if (escalationAVG > 15 && escalationAVG <= 19)
                    {
                        riskPercentage = 65;
                    }
                    else if (escalationAVG > 19 && escalationAVG <= 23)
                    {
                        riskPercentage = 73;
                    }
                    else if (escalationAVG > 23 && escalationAVG <= 29)
                    {
                        riskPercentage = 92.6;
                    }
                    break;

                case "Premier":
                    escalationAVG = hoursResult - responseAvg;

                    if (escalationAVG >= 2 && escalationAVG <=5)
                    {
                        riskPercentage = 25.1;
                    }
                    else if (escalationAVG >= 5 && escalationAVG <= 9)
                    {
                        riskPercentage = 48.2;
                    }
                    else if (escalationAVG > 9 && escalationAVG <=12)
                    {
                        riskPercentage = 66.6;
                    }
                    else if (escalationAVG > 12 && escalationAVG <= 15)
                    {
                        riskPercentage = 84.9;
                    }
                    else if (escalationAVG > 15 && escalationAVG <= 19)
                    {
                        riskPercentage = 91.7;
                    }
                    else if(escalationAVG > 19)
                    {
                        riskPercentage = 97.9;
                    }
                    break;
            }
            return riskPercentage;
        }

        public double failureToMeetSLA(int id)
        {
            //Get ticket per id received from Ajax call
            Ticket ticket = ticketRepo.GetTicketByID(id);

            //Get open hour and current hour to perform the hours diff calculation
            DateTime hourTicketOpened = ticket.Date + ticket.OpenTime;
            DateTime currentHour = DateTime.Now;

            //Obtain hours value
            TimeSpan hoursDiff = Convert.ToDateTime(currentHour).Subtract(hourTicketOpened);
            hoursResult = hoursDiff.TotalHours;

            var getSLAResponseTime = (from a in db.Tickets
                                     join b in db.Customers
                                     on a.Id_Customer equals b.Id
                                     join c in db.CustomerSLAS
                                     on b.Id equals c.IdCustomer
                                     join d in db.SLAS
                                     on c.IdSLA equals d.ID
                                     where a.Id == id
                                     select new { d.SupportType, d.ResolutionTimeAverage });

            int slaAvg = Convert.ToInt32(getSLAResponseTime.Select(y => y.ResolutionTimeAverage).First());
            string supportType = getSLAResponseTime.Select(x => x.SupportType).First();
            double hoursTakenSoFar = 0;

            switch (supportType)
            {
                case "Standard":
                    hoursTakenSoFar = hoursResult - slaAvg;

                    if (hoursTakenSoFar >= -7 && hoursTakenSoFar <= -5)
                    {
                        riskPercentageSLAFailure = 2;
                    }
                    else if (hoursTakenSoFar >= -5 && hoursTakenSoFar <= -3)
                    {
                        riskPercentageSLAFailure = 15;
                    }
                    else if (hoursTakenSoFar >= -3 && hoursTakenSoFar <= -1)
                    {
                        riskPercentageSLAFailure = 54;
                    }
                    else if (hoursTakenSoFar >= 0)
                    {
                        riskPercentageSLAFailure = 100;
                    }
                    break;

                case "Premier":
                    hoursTakenSoFar = hoursResult - slaAvg;

                    if (hoursTakenSoFar >= -7 && hoursTakenSoFar <= -5)
                    {
                        riskPercentageSLAFailure = 5;
                    }
                    else if (hoursTakenSoFar >= -5 && hoursTakenSoFar <= -3)
                    {
                        riskPercentageSLAFailure = 30;
                    }
                    else if (hoursTakenSoFar >= -3 && hoursTakenSoFar <= -1)
                    {
                        riskPercentageSLAFailure = 78;
                    }
                    else if (hoursTakenSoFar >= 0)
                    {
                        riskPercentageSLAFailure = 100;
                    }
                    break;
            }
            return riskPercentageSLAFailure;
        }

        public double periodicUpdateRisk(int id)
        {
            DateTime currentHour = DateTime.Now;

            TicketActivity getLastActivityOnTicket = db.TicketActivities.Where(x => x.idTicket == id)
                                                     .ToArray().LastOrDefault();

            if (getLastActivityOnTicket == null)
            {
                return 0;
            }
            else
            {
                DateTime LastActivity = getLastActivityOnTicket.Date + getLastActivityOnTicket.Time;

                //Obtain hours value
                TimeSpan hoursDiff = Convert.ToDateTime(currentHour).Subtract(LastActivity);
                hoursResult = hoursDiff.TotalHours;
                riskPeriodicUpdate = Math.Round(hoursResult,2);
                return riskPeriodicUpdate;
            }
        }
        #endregion

        #region SendMail Sendgrid Methods

        private void SendEmail()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("notifications@plusb.com", "PlusB Service Desk Notification");
            var subject = "Status update in incident No.  " + IDTicket;
            var to = new EmailAddress(emailToSend, "Customer");
            var plainTextContent = "Incident Status update";
            var htmlContent = "<!doctype html> " +
                                 "<html>" +
                                   "<body style='background-color:#85A885;font-family:sans-serif;font-size:14px;-webkit-font-smoothing:antialiased;line-height:1.4;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%;'>" +
                                     "<br/><br/><table border='0' cellpadding='0'cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                      " <tr>" +
                                         "<td>&nbsp;</td>" +
                                         "<td style='display:block;margin:0 auto !important;max-width:580px;padding:10px;width:580px;'>" +
                                           "<div style='box-sizing:border-box;display: block;margin:0 auto;max-width:580px;padding:10px;'>" +
                                             "<table style='background:#ffffff;border-radius:3px;width:100%;border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                               "<tr>" +
                                                 "<td style='box-sizing:border-box;padding:20px;'>" +
                                                   "<table border='0' cellpadding='0'cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                                     "<tr>" +
                                                       "<td style='font-family:sans-serif;font-size:14px;vertical-align:top;'>" +
                                                         "<p>Hi there,</p>" +
                                                         "<p>Status update occurred in an incident where you are owner. The user " + user.Email + " just updated to: </p>" +
                                                         "<table border='0' cellpadding='0'cellspacing='0'>" +
                                                           "<tbody>" +
                                                             "<tr>" +
                                                               "<td align='left'>" +
                                                                 "<table border='0' cellpadding='0'cellspacing='0'>" +
                                                                "   <tbody>" +
                                                               "      <tr>" +
                                                              "         <td style='text-align:center;'><a><strong>" + statusToSend + "</strong></a></td>" +
                                                             "        </tr>" +
                                                            "       </tbody>" +
                                                           "      </table>" +
                                                          "     </td>" +
                                                         "    </tr>" +
                                                        "   </tbody>" +
                                                       "  </table>" +
                                                      "   <br/>" +
                                                      "   <p style='font-size:12px;'>This is an automatic email sent by the PlusB Consulting Service Desk system.</p>" +
                                                     "    <p style='font-size:12px;'>Thanks for being part of our business!</p>" +
                                                    "   </td>" +
                                                   "  </tr>" +
                                                 "  </table>" +
                                               "  </td>" +
                                              " </tr>" +
                                             "</table>" +
                                             "<br/>" +
                                             "<div style='clear:both;margin-top:10px;width:100%;'>" +
                                               "<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                                 "<tr><br/>" +
                                                   "<td style='padding-bottom:10px;padding-top:10px;color:#000;font-size:12px;'>" +
                                                     "<span style='color:#000;font-size:12px;'>PlusB Consulting Escazú, San José Costa Rica</span>" +
                                                    " <br/> Don't like these emails? <a style='color:#000;font-size:12px;'>Unsubscribe</a>." +
                                                  " </td>" +
                                                 "</tr>" +
                                                 "<tr>" +
                                                   "<td style='padding-bottom:10px;padding-top:10px;text-decoration:none;color:#000;font-size:12px;'>" +
                                                     "Powered by <a style='color:#000;font-size:12px;'>PlusB Consulting S.A</a>." +
                                                   "</td>" +
                                                 "</tr>" +
                                               "</table>" +
                                             "</div" +
                                           "</div>" +
                                         "</td>" +
                                         "<td style='color:#000;font-size:12px;'>&nbsp;</td>" +
                                       "</tr>" +
                                     "</table>" +
                                  "</body>" +
                                 "</html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
        }

        private void sendSurveyEmail()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("notifications@plusb.com", "PlusB Service Desk Survey");
            var subject = "Satisfaction survey about incident No.  " + IDTicket;
            var to = new EmailAddress(emailToSend, "Customer");
            var plainTextContent = "Satisfaction survey";
            var htmlContent = "<!doctype html> " +
                                 "<html>" +
                                   "<body style='background-color:#85A885;font-family:sans-serif;font-size:14px;-webkit-font-smoothing:antialiased;line-height:1.4;margin:0;padding:0;-ms-text-size-adjust:100%;-webkit-text-size-adjust:100%;'>" +
                                     "<br/><br/><table border='0' cellpadding='0'cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                      " <tr>" +
                                         "<td>&nbsp;</td>" +
                                         "<td style='display:block;margin:0 auto !important;max-width:580px;padding:10px;width:580px;'>" +
                                           "<div style='box-sizing:border-box;display: block;margin:0 auto;max-width:580px;padding:10px;'>" +
                                             "<table style='background:#ffffff;border-radius:3px;width:100%;border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                               "<tr>" +
                                                 "<td style='box-sizing:border-box;padding:20px;'>" +
                                                   "<table border='0' cellpadding='0'cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                                     "<tr>" +
                                                       "<td style='font-family:sans-serif;font-size:14px;vertical-align:top;'>" +
                                                         "<p>Hi there,</p>" +
                                                         "<p>You were invited to evaluate the service provided by the consultant in charge of the incidente.</p>" +
                                                         "<p>Please click the follwoing link</p>"+
                                                         "<table border='0' cellpadding='0'cellspacing='0'>" +
                                                           "<tbody>" +
                                                             "<tr>" +
                                                               "<td align='left'>" +
                                                                 "<table border='0' cellpadding='0'cellspacing='0'>" +
                                                                "   <tbody>" +
                                                               "      <tr>" +
                                                              "         <td style='text-align:center;'><a>http://localhost:65158/Surveys/Create/"+IDTicket+"</a></td>" +
                                                             "        </tr>" +
                                                            "       </tbody>" +
                                                           "      </table>" +
                                                          "     </td>" +
                                                         "    </tr>" +
                                                        "   </tbody>" +
                                                       "  </table>" +
                                                      "   <br/>" +
                                                      "   <p style='font-size:12px;'>This is an automatic email sent by the PlusB Consulting Service Desk system.</p>" +
                                                     "    <p style='font-size:12px;'>Thanks for being part of our business!</p>" +
                                                    "   </td>" +
                                                   "  </tr>" +
                                                 "  </table>" +
                                               "  </td>" +
                                              " </tr>" +
                                             "</table>" +
                                             "<br/>" +
                                             "<div style='clear:both;margin-top:10px;width:100%;'>" +
                                               "<table border='0' cellpadding='0' cellspacing='0' style='border-collapse:separate;mso-table-lspace:0pt;mso-table-rspace:0pt;width:100%;'>" +
                                                 "<tr><br/>" +
                                                   "<td style='padding-bottom:10px;padding-top:10px;color:#000;font-size:12px;'>" +
                                                     "<span style='color:#000;font-size:12px;'>PlusB Consulting Escazú, San José Costa Rica</span>" +
                                                    " <br/> Don't like these emails? <a style='color:#000;font-size:12px;'>Unsubscribe</a>." +
                                                  " </td>" +
                                                 "</tr>" +
                                                 "<tr>" +
                                                   "<td style='padding-bottom:10px;padding-top:10px;text-decoration:none;color:#000;font-size:12px;'>" +
                                                     "Powered by <a style='color:#000;font-size:12px;'>PlusB Consulting S.A</a>." +
                                                   "</td>" +
                                                 "</tr>" +
                                               "</table>" +
                                             "</div" +
                                           "</div>" +
                                         "</td>" +
                                         "<td style='color:#000;font-size:12px;'>&nbsp;</td>" +
                                       "</tr>" +
                                     "</table>" +
                                  "</body>" +
                                 "</html>";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
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
