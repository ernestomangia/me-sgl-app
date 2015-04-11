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
            Provincia = new ProvinciaViewModel();
        }

        public LocalidadViewModel(LocalidadDominio localidad)
        {
            Id = localidad.Id;
            Nombre = localidad.Nombre;
            Provincia = new ProvinciaViewModel(localidad.Provincia);
            ProvinciaId = localidad.Provincia.Id;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Nombre { get; set; }
        
        public ProvinciaViewModel Provincia { get; set; }

        [Display(Name = "Provincia", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public long ProvinciaId { get; set; }

        public SelectList Provincias { get; set; }

        #endregion
    }
}