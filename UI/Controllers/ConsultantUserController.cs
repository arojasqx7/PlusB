using Domain.DAL;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Persistence.Repositories;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Models;
using UI.toastr;

namespace UI.Controllers
{
    public class ConsultantUserController : Controller
    {
        private ApplicationUserManager _userManager;
        private PlusBContext db = new PlusBContext();
        private IConsultantsRepository consultantUserRepo;
        private ITicketsRepository ticketRepo;

        public ConsultantUserController()
        {
            this.consultantUserRepo = new ConsultantsRepository(new PlusBContext());
            this.ticketRepo = new TicketsRepository(new PlusBContext());
        }

        // GET: ConsultantUser
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            ListOfCountries();
            var consultants = from s in consultantUserRepo.GetConsultants()
                              where !s.FirstName.Contains("Unassigned")
                              select s;
            return View(consultants.ToList());
        }

        [Authorize(Roles = "Administrator")]
        public PartialViewResult Create()
        {
            ConsultantUserModel consultantUser = new ConsultantUserModel();
            return PartialView("_createConsultant", consultantUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ConsultantUserModel model)
        {    
            //insert into table Consultant        
            var ConsultantDetails = new Consultant {
                ID = model.ID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                IdNumber = model.IdNumber,
                Gender = model.Gender,
                Email = model.UserName,
                Pais = model.Pais,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                JobTitle = model.JobTitle,
                Education = model.Education,
                HireDate = model.HireDate
            };
            using (var context = new PlusBContext())
            {
                context.Consultants.Add(ConsultantDetails);
                context.SaveChanges();
            }
            //ends of insert consultant post

            //insert into AspNetUser table to login the user next time and assign consultant claim
            var Password = model.Password;
            var lastConsultantId = (from i in consultantUserRepo.GetConsultants()
                                    orderby i.ID descending
                                    select i.ID).First();

            var consultantUser = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.UserName,
                ConsultantID = lastConsultantId.ToString()
            };

            var CreateConsultantUser = UserManager.Create(consultantUser, Password);
            var roleConsultant = UserManager.AddToRole(consultantUser.Id, "Consultant");
            return RedirectToAction("Index");
        }

        // GET: Consultants/Delete/5
        public ActionResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            Consultant consultant = consultantUserRepo.GetConsultantByID(id);
            return PartialView("PartialConsultants/_deleteConsultant", consultant);
        }

        // POST: Consultants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Consultant consultant = consultantUserRepo.GetConsultantByID(id);

            var ConsultantToDeleteTickets = db.Tickets
                                            .Where(x => x.Id_Consultant == consultant.ID
                                            && x.Status != "Closed")
                                            .Select(x => x.Id)
                                            .ToList();

            foreach(var i in ConsultantToDeleteTickets)
            {
                Ticket ticket = ticketRepo.GetTicketByID(i);
                ticket.Id_Consultant = 1;
                ticketRepo.UpdateTicket(ticket);
                ticketRepo.Save();
            }

            consultantUserRepo.DeleteConsultant(id);
            consultantUserRepo.Save();

            ApplicationUser user = UserManager.Users.FirstOrDefault(u => u.ConsultantID == consultant.ID.ToString());
            UserManager.Delete(user);
            this.AddToastMessage("Remove Consultant","Consultant has been removed successfully.",ToastType.Success);

            
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

        #region public ApplicationUserManager UserManager
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ??HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        #endregion

    }
}