using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class CustomerSLA
    {
        public int ID { get; set; }

        [ForeignKey("Customer")]
        public int IdCustomer { get; set; }

        [ForeignKey("SLA")]
        public int IdSLA { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual SLA SLA { get; set; }
    }
}
