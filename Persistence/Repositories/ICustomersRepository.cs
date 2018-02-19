using Domain.Entities;
using System;
using System.Collections.Generic;

namespace Persistence.Repositories
{
    public interface ICustomersRepository:IDisposable
    {
        IEnumerable<Customer> GetCustomers();
        Customer GetCustomerByID (int customerId);
        void InsertCustomer (Customer customer);
        void DeleteCustomer (int customerId);
        void UpdateCustomer(Customer customer);
        void Save();
    }
}
  