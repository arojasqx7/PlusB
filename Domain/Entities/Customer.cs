using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string CompanyName { get; set; }

        [Index(IsUnique = true)]
        public string CompanyId { get; set; }

        public string CompanyDescription { get; set; }

        public string ManagerName { get; set; }

        public string Country { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string SupportType { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime InitialDate { get; set; }

    }
}
