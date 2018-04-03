using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class ConsultantKPI
    {
        public int ID { get; set; }

        [ForeignKey("Consultant")]
        public int idConsultant { get; set; }

        [ForeignKey("KPI")]
        public int idKPI { get; set; }

        public virtual Consultant Consultant { get; set; }
        public virtual KPI KPI { get; set; }
    }
}
