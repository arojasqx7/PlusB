using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Severity
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public string SeverityName { get; set; }

        public int SeverityNumber { get; set; }
    }
}
