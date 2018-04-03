using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using UI.toastr;
using log4net;
using System;

namespace UI.Controllers
{
    public class CustomerSLAsController : Controller
    {
        private PlusBContext db = new PlusBContext();
        ILog logger = LogManager.GetLogger(typeof(CustomerSLAsController));

        public ActionResult Index()
        {
            var customerSLAS = db.CustomerSLAS.Include(c => c.Customer).Include(c => c.SLA);
            return View(customerSLAS.ToList());
        }

        // GET: CustomerSLAs/Create
        [Authorize(Roles ="Administrator")]
        public ActionResult Create(int idCustomer)
        {
            IEnumerable<CustomerSLA> SLAbyCustomer = db.CustomerSLAS.Where(x => x.IdCustomer.Equals(idCustomer)).ToList();
            ViewBag.IdSLA = new SelectList(db.SLAS.OrderBy(x=>x.Name), "Id", "Name");
            return View(SLAbyCustomer);
        }

        // POST: CustomerSLAs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,IdCustomer,IdSLA")] CustomerSLA customerSLA)
        {
            try
            { 
                if (ModelState.IsValid)
                {
                    var SLAExists = db.CustomerSLAS.Where(x=>x.IdCustomer.Equals(customerSLA.IdCustomer) && x.IdSLA.Equals(customerSLA.IdSLA)).Count();
                    if (SLAExists == 0)
                    {
                        db.CustomerSLAS.Add(customerSLA);
                        db.SaveChanges();
                        this.AddToastMessage("Assign SLA", "SLA successfully assigned to company.", ToastType.Success);
                        return RedirectToAction("Create", "CustomerSLAs", new { IdCustomer = customerSLA.IdCustomer });
                    }
                    else
                    {
                        this.AddToastMessage("Assign SLA", "This SLA already exists for this company, please verify", ToastType.Error);
                        return RedirectToAction("Create", "CustomerSLAs", new { IdCustomer = customerSLA.IdCustomer });
                    }
                    
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            ViewBag.IdCustomer = new SelectList(db.Customers, "Id", "CompanyName", customerSLA.IdCustomer);
            ViewBag.IdSLA = new SelectList(db.SLAS, "ID", "Name", customerSLA.IdSLA);
            return View(customerSLA);
        }

        // GET: CustomerSLAs/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int id)
        {
            CustomerSLA sla = db.CustomerSLAS.Find(id);
            db.CustomerSLAS.Remove(sla);
            db.SaveChanges();
            return RedirectToAction("Create", "CustomerSLAs", new { IdCustomer = sla.IdCustomer});
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
