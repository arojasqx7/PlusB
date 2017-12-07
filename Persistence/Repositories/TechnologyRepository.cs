using Domain.DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Persistence.Repositories
{
    public class TechnologyRepository : ITechnologyRepository, IDisposable
    {
        private PlusBContext context;

        // Initiliaze constructor
        public TechnologyRepository(PlusBContext context)
        {
            this.context = context;
        }

        // Create methods to insert in controller
        public IEnumerable<Technology> GetTechnologies()
        {
            return context.Technologies.ToList();
        }

        public Technology GetTechnologyByID(int id)
        {
            return context.Technologies.Find(id);
        }

        public void InsertTechnology(Technology technology)
        {
            context.Technologies.Add(technology);
        }

        public void DeleteTechnology(int technologyID)
        {
            Technology technology = context.Technologies.Find(technologyID);
            context.Technologies.Remove(technology);
        }

        public void UpdateTechnology(Technology technology)
        {
            context.Set<Technology>().AddOrUpdate(technology);
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