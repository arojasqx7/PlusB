using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Persistence.Repositories
{
    public interface IImpactRepository:IDisposable
    {
        IEnumerable<Impact> GetImpacts();
        Impact GetImpactByID(int impactId);
        void InsertImpact(Impact impact);
        void DeleteImpact(int impactID);
        void UpdateImpact(Impact impact);
        void Save();
    }
}
