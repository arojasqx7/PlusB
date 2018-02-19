using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Impact
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string ImpactName { get; set; }

        public int ImpactNumber { get; set; }
    }
}
