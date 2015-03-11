using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            Provincia = localidad.Provincia.Nombre;
        }

        #endregion

        #region Properties

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Provincia { get; set; }
        public SelectList Provincias { get; set; }
        [DisplayAttribute(Name = "Provincia")]
        public int ProvinciaId { get; set; }

        #endregion
    }
}