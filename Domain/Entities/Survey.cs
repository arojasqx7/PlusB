using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Survey
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        public DateTime DateSent { get; set; }

        public string Answer1 { get; set; }

        public string Answer2 { get; set; }

        public string Answer3 { get; set; }

        public int Answer4 { get; set; }

        public string Comments { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateAnswered { get; set; }

        public int IsAnswered { get; set; }

        [ForeignKey("Ticket")]
        public int idTicket { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
