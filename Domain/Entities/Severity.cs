using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Severity
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        [StringLength(450)]
        public string SeverityName { get; set; }

        public int SeverityNumber { get; set; }
    }
}
