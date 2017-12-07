using Domain.DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Persistence.Repositories
{
    public class SeveritiesRepository: ISeverityRepository, IDisposable
    {
        private PlusBContext context;

        // Initiliaze constructor
        public SeveritiesRepository(PlusBContext context)
        {
            this.context = context;
        }

        // Create methods to insert in controller
        public IEnumerable<Severity> GetSeverities()
        {
            return context.Severities.ToList();
        }

        public Severity GetSeverityByID(int id)
        {
            return context.Severities.Find(id);
        }

        public void InsertSeverity(Severity severity)
        {
            context.Severities.Add(severity);
        }

        public void DeleteSeverity(int severityID)
        {
            Severity severity = context.Severities.Find(severityID);
            context.Severities.Remove(severity);
        }

        public void UpdateSeverity(Severity severity)
        {
            context.Set<Severity>().AddOrUpdate(severity);
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
