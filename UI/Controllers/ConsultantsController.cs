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
        public ActionResult Index()
        {
            ListOfCountries();
            var consultants = from s in consultantRepo.GetConsultants()
                               select s;
            return View(consultants.ToList());
        }

        // GET: Consultants/Create
        public PartialViewResult Create()
        {
            return PartialView("_createConsultant");
        }

        // POST: Consultants/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FirstName,LastName,DateOfBirth,IdNumber,Gender,Email,Pais,Address,PhoneNumber,JobTitle,HireDate")] Consultant consultant)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    consultantRepo.InsertConsultant(consultant);
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
        public ActionResult Edit([Bind(Include = "ID,FirstName,LastName,DateOfBirth,IdNumber,Gender,Email,Pais,Address,PhoneNumber,JobTitle,HireDate")] Consultant consultant)
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

        // GET: Consultants/Delete/5
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Consultant consultant = consultantRepo.GetConsultantByID(id);
            return PartialView("PartialConsultants/_deleteConsultant", consultant);
        }

        // POST: Consultants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Consultant consultant = consultantRepo.GetConsultantByID(id);
            consultantRepo.DeleteConsultant(id);
            consultantRepo.Save();
            return Json(new { success = true });
        }

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
