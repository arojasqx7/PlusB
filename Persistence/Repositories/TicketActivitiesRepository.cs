using Domain.DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Persistence.Repositories
{
    public class TicketActivitiesRepository : ITicketActivityRepository, IDisposable
    {
        private PlusBContext context;

        // Initiliaze constructor
        public TicketActivitiesRepository(PlusBContext context)
        {
            this.context = context;
        }

        // Create methods to insert in controller
        public IEnumerable<TicketActivity> GetTicketActivities()
        {
            return context.TicketActivities.ToList();
        }

        public IEnumerable<TicketActivity> GetActivitiesByTicketId(int id)
        {
            return context.TicketActivities.Where(x=>x.idTicket.Equals(id));
        }

        public void InsertActivity(TicketActivity activity)
        {
            context.TicketActivities.Add(activity);
        }

        public void DeleteActivity(int activityId)
        {
            TicketActivity activity = context.TicketActivities.Find(activityId);
            context.TicketActivities.Remove(activity);
        }

        public void UpdateActivity(TicketActivity activity)
        {
            context.Set<TicketActivity>().AddOrUpdate(activity);
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
