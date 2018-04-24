using Domain.DAL;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using UI.Extensions;
using UI.toastr;

namespace UI.Controllers
{
    public class ReportsController : Controller
    {
        private PlusBContext db = new PlusBContext();
        private ApplicationUserManager _userManager;
        private static List<Ticket> ticketHistoryConsultantList = new List<Ticket>();
        private static List<Ticket> ticketHistoryCustomerList   = new List<Ticket>();

        [Authorize(Roles = "Consultant")]
        public ActionResult IncidentHistoryConsultant()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Consultant")]
        public ViewResult IncidentHistoryConsultant(DateTime DateFrom, DateTime DateTo)
        {
            if (DateFrom > DateTo)
            {
                this.AddToastMessage("Date range validation", "Date From is greather than Date To, please verify.", ToastType.Error);
                return View();
            }
            else
            {
                int idConsultant = int.Parse(User.Identity.GetConsultantId());

                var ticketsHistorybyRangeConsultant = db.Tickets
                                                     .Where(report => report.Date >= DateFrom
                                                      && report.Date <= DateTo
                                                      && report.Id_Consultant == idConsultant)
                                                     .OrderBy(report => report.Date).ThenBy(report => report.OpenTime)
                                                     .AsNoTracking()
                                                     .ToList();

                ViewBag.TotalTickets = ticketsHistorybyRangeConsultant.Count();
                ticketHistoryConsultantList = ticketsHistorybyRangeConsultant;

                return View(ticketsHistorybyRangeConsultant);
            }
        }

        [Authorize(Roles = "Customer")]
        public ActionResult IncidentHistoryCustomer()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public ViewResult IncidentHistoryCustomer(DateTime DateFrom, DateTime DateTo)
        {
            if (DateFrom > DateTo)
            {
                this.AddToastMessage("Date range validation", "Date From is greather than Date To, please verify.", ToastType.Error);
                return View();
            }
            else
            {
                var user = UserManager.FindById(User.Identity.GetUserId());

                var ticketsHistorybyRange =  db.Tickets
                                            .Where(report => report.Date >= DateFrom
                                             && report.Date <= DateTo
                                             && report.Creator == user.Email)
                                             .OrderBy(report => report.Date).ThenBy(report => report.OpenTime)
                                             .AsNoTracking()
                                             .ToList();

                ViewBag.TotalTickets = ticketsHistorybyRange.Count();
                ticketHistoryCustomerList = ticketsHistorybyRange;

                return View(ticketsHistorybyRange);
            }
        }

        public void exportIncidentHistoryCustomerToCSV()
        {
            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = ticketHistoryCustomerList;     
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=IncidentHistoryCustomer.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        public void exportIncidentHistoryConsultantToCSV()
        {
            var grid = new System.Web.UI.WebControls.GridView();
            grid.DataSource = ticketHistoryConsultantList;
            grid.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=IncidentHistoryConsultant.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
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