using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class KnowledgeBase
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public string Solution { get; set; }

        public string SpecialDetails { get; set; }

        public int IdTicket { get; set; }

        public virtual Ticket Ticket { get; set; }

    }
}
