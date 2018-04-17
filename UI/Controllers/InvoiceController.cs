using Domain.DAL;
using log4net;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using UI.toastr;
using System.Net;

namespace UI.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class InvoiceController : Controller
    {
        ILog logger = LogManager.GetLogger(typeof(InvoiceController));
        private PlusBContext db = new PlusBContext();
        
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

            return View();
        }

        //Method to call and consume CATS web service
        public void Cats()
        {
            host_PlusB.ZSER_TIME_SHEET sapService = new host_PlusB.ZSER_TIME_SHEET();
            sapService.Credentials = new NetworkCredential("AROJAS", "California01");

            // Prepare the parameters
            var param = new host_PlusB.ZesCrmTimeCats();
            param.IdTicket = "3";
            param.IdConsultor = "4";
            param.IdCliente = "8";
            param.Fecha = "2018-02-09";
            param.Horas = 40;

            var datos = new host_PlusB.ZcrmTimeSheet();
            datos.DatosEntrada = param;
            // Call SAP service
            host_PlusB.ZcrmTimeSheetResponse response = sapService.ZcrmTimeSheet(datos);
        }
    }
}