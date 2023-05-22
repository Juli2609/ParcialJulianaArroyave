using System.ComponentModel.DataAnnotations;

namespace ParcialJulianaArroyave.DAL.Entities
{
    public class Ticket
    {
        [Display(Name ="Código")]
        [Key]
        public Guid Id { get; set; }

        [Display(Name ="Fecha de Uso")]
        public DateTime? UseDate { get; set; }

        [Display(Name ="¿Ya se uso?")]
        public bool? IsUsed { get; set; }

        [Display(Name ="Puerta de entrada")]
        public string? EntranceGate { get; set; }
    }
}
