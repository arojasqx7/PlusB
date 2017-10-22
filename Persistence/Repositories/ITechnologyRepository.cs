using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Repositories
{
    public interface ITechnologyRepository: IDisposable
    {
        IEnumerable<Technology> GetTechnologies();
        Technology GetTechnologyByID(int technologyId);
        void InsertTechnology(Technology technology);
        void DeleteTechnology(int technologyID);
        void UpdateTechnology(Technology technology);
        void Save();
    }
}
