using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using UI.Extensions;
using System.Data.Entity.Infrastructure;
using log4net;
using System.Data.SqlClient;
using UI.toastr;

namespace UI.Controllers
{
    [Authorize(Roles = "Administrator, Consultant")]
    public class KnowledgeBasesController : Controller
    {
        private PlusBContext db = new PlusBContext();
        ILog logger = LogManager.GetLogger(typeof(KnowledgeBasesController));
        private int idConsultantLogged;
        private static int currentArticleId;

        // GET: KnowledgeBases
        public ActionResult Index()
        {
            var generalCount = db.KnowledgeBases.Where(x => x.Category == "General");
            var accSettingsCount = db.KnowledgeBases.Where(x => x.Category == "Account Settings");
            var configurationsCount = db.KnowledgeBases.Where(x => x.Category == "Configurations");
            var securityCount = db.KnowledgeBases.Where(x => x.Category == "Security");

            // ViewBags for counters 
            ViewBag.generalCount = generalCount.Count();
            ViewBag.accSettingsCount = accSettingsCount.Count();
            ViewBag.configurationsCount = configurationsCount.Count();
            ViewBag.securityCount = securityCount.Count();

            //ViewBags for data in foreachs 
            ViewBag.GeneralList = generalCount.Take(5).ToList();
            ViewBag.accSettList = accSettingsCount.Take(5).ToList();
            ViewBag.ConfigurationList = configurationsCount.Take(5).ToList();
            ViewBag.SecurityList = securityCount.Take(5).ToList();

            return View();
        }

        public ActionResult singleArticle(int id)
        {
            currentArticleId = id;
            KnowledgeBase knowledgeBase = db.KnowledgeBases.Find(id);
            return View(knowledgeBase);
        }

        public ActionResult singleCategory(int id)
        {
            KnowledgeBase knowledgeBase = db.KnowledgeBases.Find(id);
            var solutionsByCategory = db.KnowledgeBases.Where(x => x.Category == knowledgeBase.Category);
            return View(solutionsByCategory.ToList());
        }

        // GET: KnowledgeBases/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KnowledgeBase knowledgeBase = db.KnowledgeBases.Find(id);
            if (knowledgeBase == null)
            {
                return HttpNotFound();
            }
            return View(knowledgeBase);
        }

        // GET: KnowledgeBases/Create
        public PartialViewResult Create()
        {
            ViewBag.IdTicket = new SelectList(db.Tickets, "Id", "ShortDescription");
            return PartialView("PartialKnowledgeBase/_createSolution");
        }

        // POST: KnowledgeBases/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,CreationDate,Category,Solution,Details,idConsultant")] KnowledgeBase knowledgeBase)
        {
            if (User.IsInRole("Administrator"))
            {
                idConsultantLogged = 1;
            }
            else
            {
                idConsultantLogged = int.Parse(User.Identity.GetConsultantId());
            }

            try
            {
                var solutionToInsert = new KnowledgeBase
                {
                    ID = knowledgeBase.ID,
                    CreationDate = knowledgeBase.CreationDate,
                    Category = knowledgeBase.Category,
                    Solution = knowledgeBase.Solution,
                    Details = knowledgeBase.Details,
                    idConsultant = idConsultantLogged
                };
                using (var context = new PlusBContext())
                {
                    db.KnowledgeBases.Add(solutionToInsert);
                    db.SaveChanges();
                    this.AddToastMessage("Knowledge Base", "Solution created successfully!", ToastType.Success);
                }
            }
            catch (DbUpdateException sqlExc)
            {
                var sqlException = sqlExc.GetBaseException() as SqlException;
                if (sqlException != null)
                {
                    logger.Error(sqlExc.ToString());
                    this.AddToastMessage("Knowledge Base", "This solution already exists, please verify.", ToastType.Error);
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction("Index");
        }

        // GET: KnowledgeBases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KnowledgeBase knowledgeBase = db.KnowledgeBases.Find(id);
            if (knowledgeBase == null)
            {
                return HttpNotFound();
            }
            return View(knowledgeBase);
        }

        // POST: KnowledgeBases/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,CreationDate,Solution,SpecialDetails,IdTicket")] KnowledgeBase knowledgeBase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(knowledgeBase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
          
            return View(knowledgeBase);
        }

        // POST: KnowledgeBases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KnowledgeBase knowledgeBase = db.KnowledgeBases.Find(id);
            db.KnowledgeBases.Remove(knowledgeBase);
            db.SaveChanges();
            this.AddToastMessage("Knowledge Base", "Solution removed successfully!", ToastType.Success);
            return RedirectToAction("Index");
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
