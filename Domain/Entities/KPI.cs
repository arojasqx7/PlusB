using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class KPI
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public string Name { get; set; }

        public string Objective { get; set; }

        public double FormulaValue { get; set; }
    }
}
