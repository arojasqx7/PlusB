﻿using Domain.DAL;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Extensions;
using Domain.Entities;
using System.Threading.Tasks;
using System.IO;

namespace UI.Controllers
{
    public class TicketAttachmentController : Controller
    {
        private ITicketsRepository ticketRepo;
        private PlusBContext db = new PlusBContext();

        public TicketAttachmentController()
        {
            this.ticketRepo = new TicketsRepository(new PlusBContext());
        }

        [Authorize(Roles = "Customer")]
        public ActionResult Create()
        {
            ViewBag.Id_Customer = int.Parse(User.Identity.GetCustomerId());
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => x.FirstName.Contains("Unassigned")), "ID", "FirstName");
            ViewBag.Id_Impact = new SelectList(db.Impacts, "Id", "ImpactName");
            ViewBag.Id_Severity = new SelectList(db.Severities, "Id", "SeverityName");
            ViewBag.Id_TaskType = new SelectList(db.TaskTypes, "Id", "TaskName");
            ViewBag.Id_Technology = new SelectList(db.Technologies, "ID", "Name");
            return View();
        }

        [Authorize(Roles = "Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TicketAttachmentModel model, IEnumerable<HttpPostedFileBase> files)
        {
            //insert into table Ticket        
            var ticketDetails = new Ticket
            {
                Id = model.Id,
                Date = model.Date,
                OpenTime = model.OpenTime,
                Id_Customer = model.Id_Customer,
                ShortDescription = model.ShortDescription,
                LongDescription = model.LongDescription,
                Environment = model.Environment,
                Id_Technology = model.Id_Technology,
                Id_Severity = model.Id_Severity,
                Id_Impact = model.Id_Impact,
                Id_TaskType = model.Id_TaskType,
                Status = model.Status,
                Id_Consultant = model.Id_Consultant
            };

            using (var context = new PlusBContext())
            {
                context.Tickets.Add(ticketDetails);
                context.SaveChanges();
            }

            try
            {
                foreach (HttpPostedFileBase file in files)
                {
                    //Methods to convert attachment to byte[]
                    Stream str = file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDetail = Br.ReadBytes((Int32)str.Length);
                    //end methods

                    //query to extract last Ticket Id
                    var lastTicketId = (from i in ticketRepo.GetTickets()
                                        orderby i.Id descending
                                        select i.Id).First();

                    var attachmentDetails = new Attachment
                    {
                        FileName = file.FileName,
                        FileContent = FileDetail,
                        IdTicket = lastTicketId
                    };

                    using (var context = new PlusBContext())
                    {
                        context.Attachments.Add(attachmentDetails);
                        context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", "Tickets");
            }
            return RedirectToAction("Index", "Tickets");
        }
    }
}