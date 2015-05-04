using System.ComponentModel.DataAnnotations;

namespace ME.Libros.Utils.Enums
{
    public enum Iva
    {
        [Display(Name = "Consumidor Final")]
        ConsumidorFinal = 0,
        Monotributista = 1,
        [Display(Name = "Responsable Inscripto")]
        ResponsableInscripto = 2,
        Exento = 3,
        [Display(Name = "No Inscripto")]
        NoInscripto = 4,
    };
}
