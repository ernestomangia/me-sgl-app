using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using ME.Libros.Utils.Enums;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
    using log4net.DateFormatter;

    public class ClienteViewModel
    {
        #region Constructor(s)

        public ClienteViewModel()
        {
            Localidad = new LocalidadViewModel();
        }

        public ClienteViewModel(ClienteDominio cliente)
        {
            Id = cliente.Id;
            FechaAlta = cliente.FechaAlta;
            Nombre = cliente.Nombre;
            Apellido = cliente.Apellido;
            Cuil = cliente.Cuil;
            FechaNacimiento = cliente.FechaNacimiento;
            Sexo = cliente.Sexo;
            Direccion = cliente.Direccion;
            Numero = cliente.Numero;
            TelefonoFijo = cliente.TelefonoFijo;
            Celular = cliente.Celular;
            Email = cliente.Email;
            Localidad = new LocalidadViewModel(cliente.Localidad);
        }

        #endregion

        #region Properties

        [Display(Name = "Codigo", ResourceType = typeof(Messages))]
        public long Id { get; set; }
        
        [Display(Name = "FechaAlta", ResourceType = typeof(Messages))]
        public DateTime FechaAlta { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Nombre { get; set; }
        
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(80, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Apellido { get; set; }

        [Display(Name = "Cuil", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        [StringLength(13, MinimumLength = 13, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        public string Cuil { get; set; }

        [Display(Name = "FechaNacimiento", ResourceType = typeof(Messages))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? FechaNacimiento { get; set; }

        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public Sexo Sexo { get; set; }

        [Display(Name = "Direccion", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerida")]
        public string Direccion { get; set; }
        
        [Display(Name = "Numero", ResourceType = typeof(Messages))]
        public string Numero { get; set; }
        
        [StringLength(250, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "StringLength")]
        public string Comentario { get; set; }

        [Display(Name = "TelefonoFijo", ResourceType = typeof(Messages))]
        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        public string TelefonoFijo { get; set; }

        [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "ExactLenght")]
        public string Celular { get; set; }
        public string Email { get; set; }
        public LocalidadViewModel Localidad { get; set; }

        public SelectList Provincias { get; set; }
        public SelectList Localidades { get; set; }

        #endregion
    }
}