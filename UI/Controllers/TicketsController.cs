using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using UI.Extensions;
using Persistence.Repositories;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace UI.Controllers
{
    public class TicketsController : Controller
    {
        private PlusBContext db = new PlusBContext();
        private ITicketsRepository ticketRepo;
        private static int IDTicket;
        private static string emailToSend;
        private static string statusToSend;

        public TicketsController()
        {
            this.ticketRepo = new TicketsRepository(new PlusBContext());
        }

        // GET: Tickets
        [Authorize(Roles ="Customer")]
        public ActionResult Index()
        { 
            int idCustomer = int.Parse(User.Identity.GetCustomerId());
            var tickets = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                          .Where(x=>x.Id_Customer.Equals(idCustomer))
                          .Where(y => !y.Status.Equals("Closed")); ;
            return View(tickets.ToList());
        }

        //Show Unassigned Tickets list 
        [Authorize(Roles = "Consultant")]
        public ActionResult UnassignedList()
        {
            var unassignedTickets = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                                    .Where(x => x.Consultant.FirstName.Equals("Unassigned"));
            return View(unassignedTickets.ToList());
        }

        //Show List of assigned tickets (Logged consultant). 
        [Authorize(Roles = "Consultant")]
        public ActionResult MyTickets()
        {
            int idConsultantLogged = int.Parse(User.Identity.GetConsultantId());
            var myTickets = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                                    .Where(x => x.Id_Consultant.Equals(idConsultantLogged))
                                    .Where(y => !y.Status.Equals("Closed"));
            return View(myTickets.ToList());
        }

        [Authorize(Roles = "Consultant")]
        public ActionResult Resolved()
        {
            int idConsultantLogged = int.Parse(User.Identity.GetConsultantId());
            var resolved = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                                    .Where(x => x.Id_Consultant.Equals(idConsultantLogged))
                                    .Where(y => y.Status.Equals("Closed"));
            return View(resolved.ToList());
        }

        [Authorize(Roles = "Customer")]
        public ActionResult resolvedIncidents()
        {
            int idCustomer = int.Parse(User.Identity.GetCustomerId());
            var resolved = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology)
                                    .Where(x => x.Id_Customer.Equals(idCustomer))
                                    .Where(y => y.Status.Equals("Closed"));
            return View(resolved.ToList());
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Customer,Consultant")]
        public ActionResult Details(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return PartialView("PartialTickets/_detailsTicket", ticket);
        }

        //   get: tickets/edit/5
        [Authorize(Roles = "Customer")]
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
        [Authorize(Roles = "Customer")]
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
            int idConsultant = int.Parse(User.Identity.GetConsultantId()); // obtain id from claims
            Ticket ticket = ticketRepo.GetTicketByID(objTicket.Id); // Get all ticket details
            ticket.Id_Consultant = idConsultant; //assigning consultantID to ticket
            ticketRepo.UpdateTicket(ticket);
            ticketRepo.Save();
           // return Json(new { success = true });
            return RedirectToAction("MyTickets");
        }

        // GET
        [Authorize(Roles = "Consultant")]
        public ActionResult Assigned(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return View(ticket);
        }

        [Authorize(Roles = "Consultant, Customer")]
        public ActionResult incidentClosed(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return View(ticket);
        }

        // GET
        [Authorize(Roles = "Customer")]
        public ActionResult incidentCreated(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return View(ticket);
        }

        [HttpPost]
        public ActionResult UpdateStatus(int id, string status)
        {
            IDTicket = id;
            Ticket ticket = ticketRepo.GetTicketByID(id);
            ticket.Status = status;
            ticketRepo.UpdateTicket(ticket);
            ticketRepo.Save();

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

        private void SendEmail()
        {
            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("notifications@plusb.com", "PlusB Service Desk Notification");
            var subject = "Status update Incident #  " + IDTicket;
            var to = new EmailAddress(emailToSend, "Customer");
            var plainTextContent = "Incident Status update";
            var htmlContent = "<p>The incident status was changed to </p><br/>" +
                                statusToSend;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = client.SendEmailAsync(msg);
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
