using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ME.Libros.Dominio.General;



namespace ME.Libros.Web.Models
{
    public class RubroViewModel
    {
        #region Constructor(s)

        public RubroViewModel()
        {
            
        }

        public RubroViewModel(RubroDominio rubro)
        {
            Id = rubro.Id;
            Nombre = rubro.Nombre;
            Descripcion = rubro.Descripcion;
        }

        
        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }

        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Nombre { get; set; }

        [StringLength(250, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public string Descripcion { get; set; }

        #endregion
    }
}