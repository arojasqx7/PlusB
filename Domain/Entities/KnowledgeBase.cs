using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class KnowledgeBase
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public string Category { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Solution { get; set; }

        public string Details { get; set; }

        [ForeignKey("Consultant")]
        public int idConsultant { get; set; }

        public virtual Consultant Consultant { get; set; }


    }
}
