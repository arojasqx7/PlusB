using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class PerformanceEvaluation
    {
        public int ID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [ForeignKey("Consultant")]
        public int idConsultant { get; set; }

        public int TotalResolvedIncidents { get; set; }

        public int TotalAssignedIncidents { get; set; }

        public double TechWeightAverage { get; set; }

        public double TotalHoursTaken { get; set; }

        [DisplayFormat(DataFormatString = "{0:N}", ApplyFormatInEditMode = true)]
        public double PerformanceAverage { get; set; }

        public virtual Consultant Consultant { get; set; }

    }
}
