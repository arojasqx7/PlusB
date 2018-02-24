using System.Linq;
using System.Web.Mvc;
using Domain.DAL;
using Domain.Entities;
using Persistence.Repositories;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using log4net;

namespace UI.Controllers
{
    public class TaskTypesController : Controller
    {
        ILog logger = LogManager.GetLogger(typeof(TaskTypesController));
        private ITaskTypesRepository taskRepo;

        public TaskTypesController()
        {
            this.taskRepo = new TaskTypesRepository(new PlusBContext());
        }

        // GET: TaskTypes
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var taskTypes = from s in taskRepo.GetTaskTypes()
                          select s;
            return View(taskTypes.ToList());
        }

        // GET: TaskTypes/Create
        public PartialViewResult Create()
        {
            return PartialView("PartialTasks/_createTask");
        }

        // POST: TaskTypes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TaskName")] TaskType taskType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    taskRepo.InsertTaskType(taskType);
                    taskRepo.Save();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
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

        // GET: TaskTypes/Edit/5
        [Authorize(Roles = "Administrator")]
        public PartialViewResult Edit(int id)
        {
            TaskType taskType = taskRepo.GetTaskTypeByID(id);
            return PartialView("PartialTasks/_editTask", taskType);
        }

        // POST: TaskTypes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TaskName")] TaskType taskType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    taskRepo.UpdateTaskType(taskType);
                    taskRepo.Save();
                    return Json(new { success = true });
                }
                catch (DbUpdateException sqlExc)
                {
                    var sqlException = sqlExc.GetBaseException() as SqlException;
                    if (sqlException != null)
                    {
                        logger.Error(sqlExc.ToString());
                        ViewBag.Message = "Record already exists.";
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return PartialView("PartialTasks/_editTask", taskType);
        }

        // GET: TaskTypes/Delete/5
        [Authorize(Roles = "Administrator")]
        public PartialViewResult Delete(bool? saveChangesError = false, int id = 0)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again.";
            }
            TaskType taskType = taskRepo.GetTaskTypeByID(id);
            return PartialView("PartialTasks/_deleteTask", taskType);
        }

        // POST: Impacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "Id,TaskName")] int id)
        {
            TaskType taskType = taskRepo.GetTaskTypeByID(id);
            taskRepo.DeleteTaskType(id);
            taskRepo.Save();
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                taskRepo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
