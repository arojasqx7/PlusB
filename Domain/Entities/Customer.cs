using System;

namespace Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        public string CompanyName { get; set; }

        public string CompanyId { get; set; }

        public string CompanyDescription { get; set; }

        public string ManagerName { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string SupportType { get; set; }

        public DateTime InitialDate { get; set; }
    }
}
