using Domain.DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Persistence.Repositories
{
    public class TaskTypesRepository:ITaskTypesRepository,IDisposable
    {
        private PlusBContext context;

        // Initiliaze constructor
        public TaskTypesRepository(PlusBContext context)
        {
            this.context = context;
        }

        // Create methods to insert in controller
        public IEnumerable<TaskType> GetTaskTypes()
        {
            return context.TaskTypes.ToList();
        }

        public TaskType GetTaskTypeByID(int id)
        {
            return context.TaskTypes.Find(id);
        }

        public void InsertTaskType(TaskType taskType)
        {
            context.TaskTypes.Add(taskType);
        }

        public void DeleteTaskType(int taskId)
        {
            TaskType taskType = context.TaskTypes.Find(taskId);
            context.TaskTypes.Remove(taskType);
        }

        public void UpdateTaskType(TaskType taskType)
        {
            context.Set<TaskType>().AddOrUpdate(taskType);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}