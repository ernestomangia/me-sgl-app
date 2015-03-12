using System.ComponentModel;
using System.Web.Mvc;

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
            Provincia = new ProvinciaViewModel(localidad.Provincia);
        }

        #endregion

        #region Properties

        [DisplayName("Código")]
        public long Id { get; set; }
        public string Nombre { get; set; }
        public ProvinciaViewModel Provincia { get; set; }
        
        public SelectList Provincias { get; set; }
        
        //[DisplayName("Provincia")]
        //public int ProvinciaId { get; set; }

        #endregion
    }
}