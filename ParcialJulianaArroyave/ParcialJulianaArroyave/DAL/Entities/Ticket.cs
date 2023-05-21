using System.ComponentModel.DataAnnotations;

namespace ParcialJulianaArroyave.DAL.Entities
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime? UseDate { get; set; }
        public bool? IsUsed { get; set; }

        [Display(Name = "Portería")]
        [MaxLength(20)]
        public string? EntraceGate { get; set; }
    }
}
