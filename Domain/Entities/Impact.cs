using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Impact
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public string ImpactName { get; set; }

        public int ImpactNumber { get; set; }
    }
}
