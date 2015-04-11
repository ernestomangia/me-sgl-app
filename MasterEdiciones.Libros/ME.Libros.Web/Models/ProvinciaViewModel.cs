using System.ComponentModel.DataAnnotations;

using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    public class ProvinciaViewModel
    {
        #region Constructor(s)
        
        public ProvinciaViewModel()
        {
        }

        public ProvinciaViewModel(ProvinciaDominio provinciaDominio)
        {
            Id = provinciaDominio.Id;
            Nombre = provinciaDominio.Nombre;
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }
        [Display(Name = "Provincia", ResourceType = typeof(Messages))]
        public string Nombre { get; set; }

        #endregion
    }
}