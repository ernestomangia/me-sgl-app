
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    using System.ComponentModel;

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

        [DisplayName("Código")]
        public long Id { get; set; }
        public string Nombre { get; set; }

        #endregion
    }
}