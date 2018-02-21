using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain.Entities
{
    public class TicketActivity
    {
        public int Id { get; set;}

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public string Activity { get; set;}

        public string User { get; set; }

        [ForeignKey("Ticket")]
        public int idTicket { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}
