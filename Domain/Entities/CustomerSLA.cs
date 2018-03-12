namespace Domain.Entities
{
    public class CustomerSLA
    {
        public int ID { get; set; }

        public int IdCustomer { get; set; }

        public int IdSLA { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual SLA SLA { get; set; }
    }
}
