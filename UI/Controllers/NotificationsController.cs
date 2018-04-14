using Domain.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UI.Models;

namespace UI.Controllers
{
    public class NotificationsController : Controller
    {
        private ApplicationUserManager _userManager;
        PlusBContext db = new PlusBContext();

        [Authorize]
        [HttpGet]
        public ActionResult notificationSettings()
        {
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());
            if (user.getNotification == 1)
            {
                ViewBag.flag = true;
            }
            else
            {
                ViewBag.flag = false;
            }
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> enableDisableNotification(bool checkResp = false)
        {
            ViewBag.notificationFlag = checkResp;
            ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());

            if (checkResp == true)
            {
                user.getNotification = 1;
                this.AddToastMessage("Notifications", "Notifications have been enabled!", toastr.ToastType.Success);
            }
            else
            {
                user.getNotification = 0;
                this.AddToastMessage("Notifications", "Notifications have been disabled!", toastr.ToastType.Warning);
            }
            await UserManager.UpdateAsync(user);

            if (User.IsInRole("Consultant"))
            {
                return RedirectToAction("consultantDashboard","Dashboard");
            }
            else if (User.IsInRole("Customer"))
            {
                return RedirectToAction("customerDashboard", "Dashboard");
            }
            else
            {
                return RedirectToAction("adminDashboard", "Dashboard");
            }
        }

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

    }
}