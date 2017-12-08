using Domain.DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public class ImpactsRepository : IImpactRepository, IDisposable
    {
        private PlusBContext context;

        // Initiliaze constructor
        public ImpactsRepository(PlusBContext context)
        {
            this.context = context;
        }

        // Create methods to insert in controller
        public IEnumerable<Impact> GetImpacts()
        {
            return context.Impacts.ToList();
        }

        public Impact GetImpactByID(int id)
        {
            return context.Impacts.Find(id);
        }

        public void InsertImpact(Impact impact)
        {
            context.Impacts.Add(impact);
        }

        public void DeleteImpact(int impactID)
        {
            Impact impact = context.Impacts.Find(impactID);
            context.Impacts.Remove(impact);
        }

        public void UpdateImpact(Impact impact)
        {
            context.Set<Impact>().AddOrUpdate(impact);
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
