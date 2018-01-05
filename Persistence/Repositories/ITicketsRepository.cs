using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Persistence.Repositories
{
    public interface ITicketsRepository:IDisposable
    {
        IEnumerable<Ticket> GetTickets();
        Ticket GetTicketByID(int ticketId);
        void InsertTicket(Ticket ticket);
        void DeleteTicket(int ticketID);
        void UpdateTicket(Ticket ticket);
        void Save();
    }
}
