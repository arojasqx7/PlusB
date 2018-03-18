using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class SLA
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string Name { get; set; }

        public string Scope { get; set; }

        public double ResolutionTimeAverage { get; set; }

        public string SupportType { get; set; }

        public string PriorityName { get; set; }

        public double ResponseTime { get; set; }

    }
}
