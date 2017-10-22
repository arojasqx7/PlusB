using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Technology
    {
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Weight { get; set; }
    }
}
