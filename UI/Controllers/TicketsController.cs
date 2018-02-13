﻿using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using UI.Extensions;
using System.Web;
using System.IO;
using Persistence.Repositories;

namespace UI.Controllers
{
    public class TicketsController : Controller
    {
        private PlusBContext db = new PlusBContext();
        private ITicketsRepository ticketRepo;

        public TicketsController()
        {
            this.ticketRepo = new TicketsRepository(new PlusBContext());
        }

        // GET: Tickets
        [Authorize(Roles ="Customer")]
        public ActionResult Index()
        { 
            int idCustomer = int.Parse(User.Identity.GetCustomerId());
            var tickets = db.Tickets.Include(t => t.Consultant).Include(t => t.Customer).Include(t => t.Impact).Include(t => t.Severity).Include(t => t.TaskType).Include(t => t.Technology).Where(x=>x.Id_Customer.Equals(idCustomer));
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
                                    .Where(x => x.Id_Consultant.Equals(idConsultantLogged));
            return View(myTickets.ToList());
        }

        // GET: Tickets/Details/5
        [Authorize(Roles = "Customer,Consultant")]
        public ActionResult Details(int id)
        {
            Ticket ticket = ticketRepo.GetTicketByID(id);
            return PartialView("PartialTickets/_detailsTicket", ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Customer")]
        public ActionResult Create()
        {
            ViewBag.Id_Customer = int.Parse(User.Identity.GetCustomerId());
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x=>x.FirstName.Contains("Unassigned")), "ID", "FirstName");
            ViewBag.Id_Impact = new SelectList(db.Impacts, "Id", "ImpactName");
            ViewBag.Id_Severity = new SelectList(db.Severities, "Id", "SeverityName");
            ViewBag.Id_TaskType = new SelectList(db.TaskTypes, "Id", "TaskName");
            ViewBag.Id_Technology = new SelectList(db.Technologies, "ID", "Name");
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public ActionResult Create([Bind(Include = "Id,Date,OpenTime,Id_Customer,ShortDescription,LongDescription,Environment,Id_Technology,Id_Severity,Id_Impact,Id_TaskType,Status,Id_Consultant,files")] Ticket ticket, HttpPostedFileBase [] files)
        {
            if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase file in files)
                {
                    //Checking file is available to save.  
                    if (file != null)
                    {
                        var InputFileName =  Path.GetFileName(file.FileName);
                        var ServerSavePath = Path.Combine(Server.MapPath("~/TicketAttachments/") + InputFileName);
                        file.SaveAs(ServerSavePath);
                    }
                }
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id_Customer = int.Parse(User.Identity.GetCustomerId());
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x=>x.FirstName.Contains("Unassigned")), "ID", "FirstName", ticket.Id_Consultant);
            ViewBag.Id_Impact = new SelectList(db.Impacts, "Id", "ImpactName", ticket.Id_Impact);
            ViewBag.Id_Severity = new SelectList(db.Severities, "Id", "SeverityName", ticket.Id_Severity);
            ViewBag.Id_TaskType = new SelectList(db.TaskTypes, "Id", "TaskName", ticket.Id_TaskType);
            ViewBag.Id_Technology = new SelectList(db.Technologies, "ID", "Name", ticket.Id_Technology);
            return View(ticket);
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
        public ActionResult Edit([Bind(Include = "Id,Date,OpenTime,Id_Customer,ShortDescription,LongDescription,Environment,Id_Technology,Id_Severity,Id_Impact,Id_TaskType,Status,Id_Consultant,files")] Ticket ticket)
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
            int idConsultant = int.Parse(User.Identity.GetCustomerId()); // obtain id from claims
            Ticket ticket = ticketRepo.GetTicketByID(objTicket.Id); // Get all ticket details
            ticket.Id_Consultant = idConsultant; //assigning consultantID to ticket
            ticketRepo.UpdateTicket(ticket);
            ticketRepo.Save();
           // return Json(new { success = true });
            return RedirectToAction("MyTickets");
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
