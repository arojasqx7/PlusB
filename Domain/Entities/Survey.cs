using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Survey
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateSent { get; set; }

        public int Question1 { get; set; }

        public int Question2 { get; set; }

        public string Question3 { get; set; }

        public string Comments { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateAnswered { get; set; }

        public int IdTicket { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
