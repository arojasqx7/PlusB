using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Persistence.Repositories
{
    public interface ITicketActivityRepository:IDisposable
    {
        IEnumerable<TicketActivity> GetTicketActivities();
        IEnumerable<TicketActivity> GetActivitiesByTicketId(int ticketId);
        void InsertActivity(TicketActivity activity);
        void DeleteActivity(int activityId);
        void UpdateActivity(TicketActivity activity);
        void Save();
    }
}
