using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using System.Globalization;
using Persistence.Repositories;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System;
using PagedList;

namespace UI.Controllers
{
    public class ConsultantsController : Controller
    {
        private IConsultantsRepository consultantRepo;

        public ConsultantsController()
        {
            this.consultantRepo = new ConsultantsRepository(new PlusBContext());
        }

        // GET: Consultants
        [Authorize(Roles ="Administrator")]
        public ViewResult Index(string currentFilter, string searchString, int? page)
        {
            ListOfCountries();
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var consultants = consultantRepo.GetConsultants()
                              .Where(x => !x.FirstName.Contains("Unassigned"));

            if (!String.IsNullOrEmpty(searchString))
            {
                consultants = consultants.Where(s => s.FirstName.Contains(searchString)
                                                || s.LastName.Contains(searchString));
            }

            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(consultants.OrderBy(x => x.FirstName).ThenBy(y=>y.LastName).ToPagedList(pageNumber, pageSize));
        }

        // GET: Consultants/Create
        public PartialViewResult Create()
        {
            return PartialView("_createConsultant");
        }

        // POST: Consultants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,DateOfBirth,IdNumber,Gender,Email,Pais,Address,PhoneNumber,JobTitle,Education,HireDate")] Consultant consultant)
        {

            // Create user
            //var consultantUser = new ApplicationUser {
            //    UserName = UserName,
            //    Email = Email
            //};
            //var CreateConsultantUser = UserManager.Create(consultantUser, Password);

            if (ModelState.IsValid)
            {
                try
                {
                    consultantRepo.InsertConsultant(consultant);
                    consultantRepo.Save();
                    return RedirectToAction("Index");
                }
                /*If the record already exists (using ID validation), as per the constraint Unique validation 
                 * implemented in the model, the debugger will run to the CATCH where the ViewBag.Message will 
                 * display a message to the View.
                 */
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

        // GET: Consultants/Edit/5
        public ActionResult Edit(int id)
        {
            ListOfCountries();
            Consultant consultant = consultantRepo.GetConsultantByID(id);
            return PartialView("PartialConsultants/_editConsultant", consultant);
        }

        // POST: Consultants/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,DateOfBirth,IdNumber,Gender,Email,Pais,Address,PhoneNumber,JobTitle,Education,HireDate")] Consultant consultant)
        {
            ListOfCountries();
                if (ModelState.IsValid)
                {
                    try
                    {
                       consultantRepo.UpdateConsultant(consultant);
                       consultantRepo.Save();
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
            return PartialView("PartialConsultants/_editConsultant", consultant);
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
                consultantRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
