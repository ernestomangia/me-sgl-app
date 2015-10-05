using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class LocalidadViewModel
    {
        #region Constructor(s)

        public LocalidadViewModel()
        {
        }

        public LocalidadViewModel(LocalidadDominio localidad)
        {
            Id = localidad.Id;
            Nombre = localidad.Nombre;
            CodigoPostal = localidad.CodigoPostal;
            Provincia = new ProvinciaViewModel(localidad.Provincia);
            ProvinciaId = localidad.Provincia.Id;
            Zona = new ZonaViewModel(localidad.Zona);
            ZonaId = localidad.Zona.Id;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [RegularExpression(@"^([a-zA-ZñÑáéíóúÁÉÍÓÚ´]+\s)*[a-zA-ZñÑáéíóúÁÉÍÓÚ´]+$", ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "OnlyLetters")]
        public string Nombre { get; set; }

        [Display(Name = "CodigoPostal", ResourceType = typeof(Messages))]
        [StringLength(5, MinimumLength = 5, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        public string CodigoPostal { get; set; }

        [Display(Name = "Provincia", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long ProvinciaId { get; set; }

        [Display(Name = "Zona", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long ZonaId { get; set; }
        
        public ProvinciaViewModel Provincia { get; set; }
        public ZonaViewModel Zona { get; set; }
        public SelectList Provincias { get; set; }
        public SelectList Zonas { get; set; }

        #endregion
    }
}