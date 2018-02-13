﻿using Domain.DAL;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Persistence.Repositories;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UI.Models;

namespace UI.Controllers
{
    public class ConsultantUserController : Controller
    {
        private ApplicationUserManager _userManager;
        private PlusBContext db = new PlusBContext();
        //private IConsultantsRepository consultantUserRepo;

        //public ConsultantUserController()
        //{
        //    this.consultantUserRepo = new ConsultantsRepository(new PlusBContext());
        //}

        // GET: ConsultantUser
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ListOfCountries();
            var consultants = from s in db.Consultants
                              where !s.FirstName.Contains("Unassigned")
                              select s;
            return View(consultants.ToList());
        }

        public PartialViewResult Create()
        {
            ConsultantUserModel consultantUser = new ConsultantUserModel();
            return PartialView("_createConsultant", consultantUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ConsultantUserModel model)
        {
            var ConsultantDetails = new Consultant {
                ID= model.ID,
                FirstName=model.FirstName,
                LastName=model.LastName,
                DateOfBirth=model.DateOfBirth,
                IdNumber=model.IdNumber,
                Gender=model.Gender,
                Email= model.Email,
                Pais=model.Pais,
                Address=model.Address,
                PhoneNumber=model.PhoneNumber,
                JobTitle=model.JobTitle,
                HireDate=model.HireDate
            };

            // Create user
            var Password = model.Password;
            string consultantID = model.ID.ToString();
            var consultantUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                ConsultantID = consultantID
            };

            using (var context = new PlusBContext())
            {
                context.Consultants.Add(ConsultantDetails);
                context.SaveChanges();              
            }
            var CreateConsultantUser = UserManager.Create(consultantUser, Password);
            return RedirectToAction("Index");
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

        #region public ApplicationUserManager UserManager
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??
                    HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

    }
}