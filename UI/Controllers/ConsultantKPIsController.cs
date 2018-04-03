using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using log4net;
using UI.toastr;

namespace UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ConsultantKPIsController : Controller
    {
        private PlusBContext db = new PlusBContext();
        ILog logger = LogManager.GetLogger(typeof(ConsultantKPIsController));

        // GET: ConsultantKPIs
        public ActionResult Index()
        {
            var consultantsKPIs = db.ConsultantsKPIs.Include(c => c.Consultant).Include(c => c.KPI);
            return View(consultantsKPIs.ToList());
        }

        // GET: ConsultantKPIs/Create
        public ActionResult Create(int idConsultant)
        {
            IEnumerable<ConsultantKPI> KPIbyConsultant = db.ConsultantsKPIs.Where(x => x.idConsultant.Equals(idConsultant)).ToList();
            ViewBag.IdKPI = new SelectList(db.KPIS.OrderBy(x => x.Name), "ID", "Name");
            return View(KPIbyConsultant);
        }

        // POST: ConsultantKPIs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,idConsultant,idKPI")] ConsultantKPI consultantKPI)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var KPIExists = db.ConsultantsKPIs.Where(x => x.idConsultant.Equals(consultantKPI.idConsultant) && x.idKPI.Equals(consultantKPI.idKPI)).Count();
                    if (KPIExists == 0)
                    {
                        db.ConsultantsKPIs.Add(consultantKPI);
                        db.SaveChanges();
                        this.AddToastMessage("Assign KPI", "KPI successfully assigned to consultant.", ToastType.Success);
                        return RedirectToAction("Create", "ConsultantKPIs", new { idConsultant = consultantKPI.idConsultant });
                    }
                    else
                    {
                        this.AddToastMessage("Assign KPI", "This KPI already exists for this consultant, please verify", ToastType.Error);
                        return RedirectToAction("Create", "ConsultantKPIs", new { idConsultant = consultantKPI.idConsultant });
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }

            ViewBag.idConsultant = new SelectList(db.Consultants, "Id", "FullName", consultantKPI.idConsultant);
            ViewBag.idKPI = new SelectList(db.SLAS, "ID", "Name", consultantKPI.idKPI);
            return View(consultantKPI);
        }

        public ActionResult Delete(int id)
        {
            ConsultantKPI kPI = db.ConsultantsKPIs.Find(id);
            db.ConsultantsKPIs.Remove(kPI);
            db.SaveChanges();
            return RedirectToAction("Create", "ConsultantKPIs", new { idConsultant = kPI.idConsultant });
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
