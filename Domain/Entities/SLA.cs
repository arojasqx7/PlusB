using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class SLA
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public string Priority { get; set; }

    }
}
