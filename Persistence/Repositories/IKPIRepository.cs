using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Persistence.Repositories
{
    public interface IKPIRepository:IDisposable
    {
        IEnumerable<KPI> GetKPIs();
        KPI GetKPIByID(int KPIId);
        void InsertKPI(KPI kpi);
        void DeleteKPI(int KPIId);
        void UpdateKPI(KPI kpi);
        void Save();
    }
}
