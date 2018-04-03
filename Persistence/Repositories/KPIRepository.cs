using Domain.DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Persistence.Repositories
{
    public class KPIRepository:IKPIRepository, IDisposable
    {
        private PlusBContext context;

        // Initiliaze constructor
        public KPIRepository(PlusBContext context)
        {
            this.context = context;
        }

        // Create methods to insert in controller
        public IEnumerable<KPI> GetKPIs()
        {
            return context.KPIS.ToList();
        }

        public KPI GetKPIByID(int id)
        {
            return context.KPIS.Find(id);
        }

        public void InsertKPI(KPI kpi)
        {
            context.KPIS.Add(kpi);
        }

        public void DeleteKPI(int kpiID)
        {
            KPI kpi = context.KPIS.Find(kpiID);
            context.KPIS.Remove(kpi);
        }

        public void UpdateKPI(KPI kpi)
        {
            context.Set<KPI>().AddOrUpdate(kpi);
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
