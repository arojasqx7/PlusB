using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using Persistence.Repositories;
using System.Collections.Generic;
using System.Globalization;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace UI.Controllers
{
    public class CustomersController : Controller
    {
        private PlusBContext db = new PlusBContext();
        private ICustomersRepository customerRepo;

        public CustomersController()
        {
            this.customerRepo = new CustomersRepository(new PlusBContext());
        }

       // GET: Customers
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ListOfCountries();
            var customers = from s in customerRepo.GetCustomers()
                            where !s.Id.Equals(1)
                            select s;
            return View(customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CompanyName,CompanyId,CompanyDescription,ManagerName,Country,Address,PhoneNumber,EmailAddress,SupportType,InitialDate")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    customerRepo.InsertCustomer(customer);
                    customerRepo.Save();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                        if (sqlException != null)
                        {
                            ViewBag.Message = "Record already exists.";
                        }
                        else
                        {
                            throw;
                        }
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Customers/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int id)
        {
            ListOfCountries();
            Customer customer = customerRepo.GetCustomerByID(id);
            return PartialView("PartialCustomers/_editCustomer", customer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CompanyName,CompanyId,CompanyDescription,ManagerName,Country,Address,PhoneNumber,EmailAddress,SupportType,InitialDate")] Customer customer)
        {
            ListOfCountries();
            if (ModelState.IsValid)
            {
                try
                {
                    customerRepo.UpdateCustomer(customer);
                    customerRepo.Save();
                    return Json(new { success = true });
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        ViewBag.Message = "Record already exists.";
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("PartialCustomers/_editCustomer", customer);
        }

        // GET: Customers/Delete/5
        [Authorize(Roles = "Administrator")]
        public PartialViewResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Customer customer = customerRepo.GetCustomerByID(id);
            return PartialView("PartialCustomers/_deleteCustomer", customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "Id,CompanyName,CompanyId,CompanyDescription,ManagerName,Country,Address,PhoneNumber,EmailAddress,SupportType,InitialDate")] int id)
        {
            Customer customer = customerRepo.GetCustomerByID(id);
            customerRepo.DeleteCustomer(id);
            customerRepo.Save();
            return Json(new { success = true });
        }

        //This method lists all countries and send them in a ViewBag.CountryList
        public void ListOfCountries()
        {
            List<string> CountryList = new List<string>();
            CultureInfo[] CInfoList = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
            foreach (CultureInfo CInfo in CInfoList)
            {
                RegionInfo R = new RegionInfo(CInfo.LCID);
                if (!(CountryList.Contains(R.EnglishName)))
                {
                    CountryList.Add(R.EnglishName);
                }
            }
            CountryList.Sort();
            ViewBag.CountryList = CountryList;
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
