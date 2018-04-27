using Domain.DAL;
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
using Microsoft.AspNet.Identity;
using log4net;
using Microsoft.AspNet.Identity.Owin;
using UI.toastr;

namespace UI.Controllers
{
    public class TicketAttachmentController : Controller
    {
        private ApplicationUserManager _userManager;
        private ITicketsRepository ticketRepo;
        private PlusBContext db = new PlusBContext();
        ILog logger = LogManager.GetLogger(typeof(TicketAttachmentController));

        public TicketAttachmentController()
        {
            this.ticketRepo = new TicketsRepository(new PlusBContext());
        }

        [Authorize(Roles = "Customer, Administrator")]
        public ActionResult Create()
        {
            if (User.IsInRole("Administrator"))
            {
                ViewBag.Id_Customer = 1;
            }
            else
            {
                ViewBag.Id_Customer = int.Parse(User.Identity.GetCustomerId());
            }
            
            ViewBag.Id_Consultant = new SelectList(db.Consultants.Where(x => x.FirstName.Contains("Unassigned")), "ID", "FirstName");
            ViewBag.Id_Impact = new SelectList(db.Impacts, "Id", "ImpactName");
            ViewBag.Id_Severity = new SelectList(db.Severities, "Id", "SeverityName");
            ViewBag.Id_TaskType = new SelectList(db.TaskTypes, "Id", "TaskName");
            ViewBag.Id_Technology = new SelectList(db.Technologies.OrderBy(x => x.Name), "ID", "Name");
            return View();
        }

        [Authorize(Roles = "Customer, Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(TicketAttachmentModel model, IEnumerable<HttpPostedFileBase> files)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
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
                Id_Consultant = model.Id_Consultant,
                Creator = user.Email,
                ClosedDate = null,
                AssignmentDate = null,
                AverageResolution=null,
                AssignmentTime=null,
                ClosedTime=null,
                TotalResolutionHours=null,
                AutoAssigned=0,
                EscalationReason=null,
                EscalationDate = null
            };

            using (var context = new PlusBContext())
            {
                context.Tickets.Add(ticketDetails);
                context.SaveChanges();
                this.AddToastMessage("Incident", "Incident created successfully.", ToastType.Success);

                var consultantEmails =  db.Consultants
                                        .Where(consultant => consultant.ID != 1)
                                        .Select(consultant => consultant.Email)
                                        .ToList();

                foreach (var email in consultantEmails)
                {
                    string body = "<b>A new Incident has been opened </b>";
                    string subject = "Consultant - New Incident created";
                    EmailManager.SendEmail(email, body, subject);
                }
            }

            try
            {
                foreach (HttpPostedFileBase file in files)
                {
                    //Methods to convert attachment to byte[]
                    String FileExt = Path.GetExtension(file.FileName).ToUpper();
                    Stream str = file.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDetail = Br.ReadBytes((Int32)str.Length);

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
                logger.Error(ex.ToString());
            }

            if (User.IsInRole("Customer"))
            {
                return RedirectToAction("Index", "Tickets");
            }
            else
            {
                return RedirectToAction("UnassignedList", "Tickets");
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
