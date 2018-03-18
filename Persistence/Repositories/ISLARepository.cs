using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Persistence.Repositories
{
    public interface ISLARepository:IDisposable
    {
        IEnumerable<SLA> GetSLAs();
        SLA GetSLAByID(int SLAId);
        void InsertSLA(SLA sla);
        void DeleteSLA(int SLAId);
        void UpdateSLA(SLA sla);
        void Save();
    }
}
