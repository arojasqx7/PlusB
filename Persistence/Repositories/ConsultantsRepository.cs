using Domain.DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Persistence.Repositories
{
    public class ConsultantsRepository : IConsultantsRepository, IDisposable
    {
        private PlusBContext context;

        // Initiliaze constructor
        public ConsultantsRepository(PlusBContext context)
        {
            this.context = context;
        }

        // Create methods to insert in controller
        public IEnumerable<Consultant> GetConsultants()
        {
            return context.Consultants.ToList();
        }

        public Consultant GetConsultantByID(int id)
        {
            return context.Consultants.Find(id);
        }

        public void InsertConsultant(Consultant consultant)
        {
            context.Consultants.Add(consultant);
        }

        public void DeleteConsultant(int consultantID)
        {
            Consultant consultant = context.Consultants.Find(consultantID);
            context.Consultants.Remove(consultant);
        }

        public void UpdateConsultant(Consultant consultant)
        {
            context.Set<Consultant>().AddOrUpdate(consultant);
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
