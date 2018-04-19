using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Technology
    {
        public int ID { get; set; }

        [Index(IsUnique = true)]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(450)]
        public string Name { get; set; }

        public string Description { get; set; }

        public int Weight { get; set; }

        public double HourPrice { get; set; }
    }
}
