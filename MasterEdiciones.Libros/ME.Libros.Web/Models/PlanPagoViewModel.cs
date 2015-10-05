using System.ComponentModel.DataAnnotations;

using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class PlanPagoViewModel
    {
        #region Constructor(s)

        public PlanPagoViewModel()
        {
        }

        public PlanPagoViewModel(PlanPagoDominio planPago)
        {
            Id = planPago.Id;
            Nombre = planPago.Nombre;
            Descripcion = planPago.Descripcion;
            CantidadCuotas = planPago.CantidadCuotas;
            MontoCuota = planPago.MontoCuota;
            Monto = planPago.Monto;
            Tipo = planPago.Tipo;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-ZñÑáéíóúÁÉÍÓÚ´0-9]+\s)*[a-zA-ZñÑáéíóúÁÉÍÓÚ´0-9]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLettersAndNumbers")]
        public string Nombre { get; set; }


        [StringLength(150, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Descripcion { get; set; }

        [Display(Name = "CantidadCuotas", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public int CantidadCuotas { get; set; }

        [Display(Name = "MontoCuota", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal MontoCuota { get; set; }

        [Display(Name = "MontoPlanPago", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Monto { get; set; }

        public TipoPlanPago Tipo { get; set; }

        public bool Modificable { get; set; }

        #endregion
    }
}