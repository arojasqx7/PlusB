using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Domain.Entities
{
    public class Attachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        [ForeignKey("Ticket")]
        public int IdTicket { get; set; }

        [NotMapped]
        [DataType(DataType.Upload)]
        [Display(Name = "Choose File(s)")]
        public HttpPostedFileBase files { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
