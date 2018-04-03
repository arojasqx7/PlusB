using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Survey
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateSent { get; set; }

        public int Answer1 { get; set; }

        public int Answer2 { get; set; }

        public string Answer3 { get; set; }

        public string Comments { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateAnswered { get; set; }

        [ForeignKey("Ticket")]
        public int idTicket { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
