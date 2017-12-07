using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class TaskType
    {
        public int Id { get; set; }

        [Index(IsUnique = true)]
        public string TaskName { get; set; }

    }
}
