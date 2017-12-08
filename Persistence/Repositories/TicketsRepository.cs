using Domain.DAL;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Persistence.Repositories
{
    public class TicketsRepository: ITicketsRepository,IDisposable
    {
        private PlusBContext context;

        // Initiliaze constructor
        public TicketsRepository(PlusBContext context)
        {
            this.context = context;
        }

        // Create methods to insert in controller
        public IEnumerable<Ticket> GetTickets()
        {
            return context.Tickets.ToList();
        }

        public Ticket GetTicketByID(int id)
        {
            return context.Tickets.Find(id);
        }

        public void InsertTicket(Ticket ticket)
        {
            context.Tickets.Add(ticket);
        }

        public void DeleteTicket(int ticketID)
        {
            Ticket ticket = context.Tickets.Find(ticketID);
            context.Tickets.Remove(ticket);
        }

        public void UpdateTicket(Ticket ticket)
        {
            context.Set<Ticket>().AddOrUpdate(ticket);
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
