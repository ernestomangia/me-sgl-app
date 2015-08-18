using System.ComponentModel.DataAnnotations;

using ME.Libros.Dominio.General;

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
            Monto = planPago.Monto;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [StringLength(100, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Nombre { get; set; }

        [StringLength(150, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Descripcion { get; set; }

        [Display(Name = "CantidadCuotas", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public int CantidadCuotas { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Monto { get; set; }

        public bool Modificable { get; set; }

        #endregion
    }
}