using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using ME.Libros.Utils.Enums;
using ME.Libros.Dominio.General;

namespace ME.Libros.Web.Models
{
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

        public long Id { get; set; }
        public DateTime FechaAlta { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [StringLength(13, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "CuilLength")]
        public string Cuil { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        [Required]
        public Sexo Sexo { get; set; }
        [Required]
        public string Direccion { get; set; }
        public string Numero { get; set; }
        public string Comentario { get; set; }
        [StringLength(10, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "TelefonoFijoLength")]
        public string TelefonoFijo { get; set; }
        [StringLength(14, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "CelularLength")]
        public string Celular { get; set; }
        public string Email { get; set; }
        public LocalidadViewModel Localidad { get; set; }

        public SelectList Provincias { get; set; }
        public SelectList Localidades { get; set; }

        #endregion
    }
}