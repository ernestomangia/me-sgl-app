using System.ComponentModel.DataAnnotations;

using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class IvaViewModel
    {
        #region Constructor(s)

        public IvaViewModel()
        {
        }

        public IvaViewModel(IvaDominio iva)
        {
            Id = iva.Id;
            Nombre = iva.Nombre;
            Alicuota = iva.Alicuota;
            HabilitarEliminar = iva.HabilitarEliminar;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Nombre { get; set; }

        [Display(Name = "Alicuota", ResourceType = typeof(Messages))]
        [DisplayFormat(DataFormatString = "{0:#.##} %")]
        public decimal Alicuota { get; set; }

        public bool HabilitarEliminar { get; set; }

        #endregion
    }
}