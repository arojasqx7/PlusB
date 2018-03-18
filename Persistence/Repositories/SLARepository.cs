using Domain.DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Persistence.Repositories
{
    public class SLARepository : ISLARepository, IDisposable
    {
        private PlusBContext context;

        // Initiliaze constructor
        public SLARepository(PlusBContext context)
        {
            this.context = context;
        }

        // Create methods to insert in controller
        public IEnumerable<SLA> GetSLAs()
        {
            return context.SLAS.ToList();
        }

        public SLA GetSLAByID(int id)
        {
            return context.SLAS.Find(id);
        }

        public void InsertSLA(SLA sla)
        {
            context.SLAS.Add(sla);
        }

        public void DeleteSLA(int SLAId)
        {
            SLA sla = context.SLAS.Find(SLAId);
            context.SLAS.Remove(sla);
        }

        public void UpdateSLA(SLA sla)
        {
            context.Set<SLA>().AddOrUpdate(sla);
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