
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

        public int Id { get; set; }

        public string Nombre { get; set; }

        #endregion
    }
}