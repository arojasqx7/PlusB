using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Persistence.Repositories
{
    public interface ISeverityRepository: IDisposable
    {
        IEnumerable<Severity> GetSeverities();
        Severity GetSeverityByID(int severityId);
        void InsertSeverity(Severity severity);
        void DeleteSeverity(int severityID);
        void UpdateSeverity(Severity severity);
        void Save();
    }
}
