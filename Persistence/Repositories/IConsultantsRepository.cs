using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Persistence.Repositories
{
    public interface IConsultantsRepository:IDisposable
    {
        IEnumerable<Consultant> GetConsultants();
        Consultant GetConsultantByID(int ConsultantId);
        void InsertConsultant(Consultant consultant);
        void DeleteConsultant(int consultantID);
        void UpdateConsultant(Consultant consultant);
        void Save();
    }
}
