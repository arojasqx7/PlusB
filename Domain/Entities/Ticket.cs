using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        [ForeignKey("Customer")]
        public int Id_Customer {get;set;}

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string Environment { get; set; }

        [ForeignKey("Technology")]
        public int Id_Technology { get; set; }

        [ForeignKey("Severity")]
        public int Id_Severity { get; set; }

        [ForeignKey("Impact")]
        public int Id_Impact { get; set; }

        [ForeignKey("TaskType")]
        public int Id_TaskType { get; set; }

        public string Status { get; set; }

        [ForeignKey("Consultant")]
        public int Id_Consultant { get; set; }

        public virtual Technology Technology { get; set; }
        public virtual Severity Severity { get; set; }
        public virtual Impact Impact { get; set; }
        public virtual TaskType TaskType { get; set; }
        public virtual Consultant Consultant { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
