using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class KPIEvaluation
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [ForeignKey("Ticket")]
        public int? idTicket { get; set; }

        [ForeignKey("Consultant")]
        public int idConsultant { get; set; }

        [ForeignKey("KPI")]
        public int idKPI{ get; set; }

        public double Score { get; set; }

        public string Range { get; set; }

        public virtual Consultant Consultant { get; set; }
        public virtual KPI KPI { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
