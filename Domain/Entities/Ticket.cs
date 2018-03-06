using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Ticket
    {
        public int Id { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public TimeSpan OpenTime { get; set; }

        [ForeignKey("Customer")]
        public int Id_Customer {get;set;}

        public string ShortDescription { get; set; }

        [DataType(DataType.MultilineText)]
        public string LongDescription { get; set; }

        [DataType(DataType.MultilineText)]
        public string Environment { get; set; }

        [ForeignKey("Technology")]
        public int Id_Technology { get; set; }

        [ForeignKey("Severity")]
        public int Id_Severity { get; set; }

        [ForeignKey("Impact")]
        public int Id_Impact { get; set; }

        [ForeignKey("TaskType")]
        public int Id_TaskType { get; set; }

        public string Status { get; set; }

        [ForeignKey("Consultant")]
        public int Id_Consultant { get; set; }

        public string Creator { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? AssignmentDate { get; set; }

        public TimeSpan? AssignmentTime { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime? ClosedDate { get; set; }

        public TimeSpan? ClosedTime { get; set; }

        public decimal? AverageResolution { get; set; }

        public virtual Technology Technology { get; set; }
        public virtual Severity Severity { get; set; }
        public virtual Impact Impact { get; set; }
        public virtual TaskType TaskType { get; set; }
        public virtual Consultant Consultant { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
