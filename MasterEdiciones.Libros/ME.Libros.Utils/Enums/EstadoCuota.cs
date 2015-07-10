using System.ComponentModel.DataAnnotations;

namespace ME.Libros.Utils.Enums
{
    public enum EstadoCuota
    {
        [Display(Name = "No vencida")]
        NoVencida = 1,
        Pagada = 2,
        Atrasada = 3,
        Parcial = 4,
    }
}
