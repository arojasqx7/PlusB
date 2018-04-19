using Domain.DAL;
using log4net;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UI.toastr;
using System.Net;
using Domain.Entities;

namespace UI.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class InvoiceController : Controller
    {
        ILog logger = LogManager.GetLogger(typeof(InvoiceController));
        private PlusBContext db = new PlusBContext();

        private static string idTicketCATS;
        private static string IdConsultorCATS;
        private static string IdClienteCATS;
        private static string fechaCATS;
        private static decimal horasCATS;

        public ActionResult invoiceList()
        {
            ViewBag.Id_Customer = new SelectList(db.Customers
                                 .Where(customer => customer.CompanyName != "PlusB Consulting")
                                 .OrderBy(customer => customer.CompanyName),"Id", "CompanyName");
            return View();
        }

        [HttpPost]
        public ViewResult invoiceList(int Id_Customer, DateTime DateFrom, DateTime DateTo)
        {
            ViewBag.Id_Customer = new SelectList(db.Customers
                                 .Where(customer => customer.CompanyName != "PlusB Consulting")
                                 .OrderBy(customer => customer.CompanyName), "Id", "CompanyName");

            if (DateFrom > DateTo)
            {
                this.AddToastMessage("Date range validation", "Date From is greather than Date To, please verify.", ToastType.Error);
                return View();
            }
            else
            {
                var ticketsByRange =  db.Tickets
                                     .Where(ticket => ticket.Id_Customer == Id_Customer
                                      && ticket.ClosedDate >= DateFrom
                                      && ticket.ClosedDate <= DateTo)
                                     .OrderBy(x => x.ClosedDate)
                                     .AsNoTracking()
                                     .ToList();
                return View(ticketsByRange);
            }
        }

        public ActionResult viewInvoiceDetails(int idTicket)
        {
            Ticket ticket = db.Tickets.Find(idTicket);

            var subtotal = ticket.Technology.HourPrice * ticket.TotalResolutionHours;
            var tax = subtotal * 0.13;
            var total = subtotal + tax;

            ViewBag.Subtotal = Math.Round(subtotal.GetValueOrDefault(), 2);
            ViewBag.Tax      = Math.Round(tax.GetValueOrDefault(), 2);
            ViewBag.Total    = Math.Round(total.GetValueOrDefault() , 2);

            //Parameters to CATS in String
            DateTime today     = DateTime.Now;
            string todayString = today.ToString("yyyy/MM/dd");

            idTicketCATS    = idTicket.ToString();
            IdConsultorCATS = ticket.Id_Consultant.ToString();
            IdClienteCATS   = ticket.Id_Customer.ToString();
            fechaCATS       = todayString;
            horasCATS       = Math.Round(Convert.ToDecimal(ticket.TotalResolutionHours), 2);

            return View(ticket);
        }

        //Method to call and consume CATS web service
        public ActionResult Cats()
        {
            ViewBag.Id_Customer = new SelectList(db.Customers
                                 .Where(customer => customer.CompanyName != "PlusB Consulting")
                                 .OrderBy(customer => customer.CompanyName), "Id", "CompanyName");

            host_PlusB.ZSER_TIME_SHEET sapService = new host_PlusB.ZSER_TIME_SHEET();
            sapService.Credentials = new NetworkCredential("AROJAS", "California01");

            // Prepare the parameters
            var param = new host_PlusB.ZesCrmTimeCats();
            param.IdTicket      = idTicketCATS;
            param.IdConsultor   = IdConsultorCATS;
            param.IdCliente     = IdClienteCATS;
            param.Fecha         = fechaCATS;
            param.Horas         = horasCATS;

            var data = new host_PlusB.ZcrmTimeSheet();
            data.DatosEntrada = param;

            // Call SAP service
            host_PlusB.ZcrmTimeSheetResponse response = sapService.ZcrmTimeSheet(data);

            logger.Info("Information inserted to SAP CATS");
            this.AddToastMessage("CATS Service","Information was successfully sent to SAP CATS!", ToastType.Success);
            return View("invoiceList");
        }
    }
}