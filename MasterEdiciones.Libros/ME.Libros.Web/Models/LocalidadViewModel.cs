using System.Collections.Generic;
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
            Nombre = localidad.Nombre;
            Provincia = new ProvinciaViewModel
            {
                Id = localidad.Provincia.Id,
                Nombre = localidad.Provincia.Nombre
            };
        }

        #endregion

        #region Properties

        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<SelectListItem> ProvinciasList { get; set; }
        public SelectList Provincias { get; set; }
        public ProvinciaViewModel Provincia { get; set; }

        #endregion
    }
}