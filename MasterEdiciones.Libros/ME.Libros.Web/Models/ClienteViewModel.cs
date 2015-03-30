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
            CalleA = cliente.CalleA;
            CalleB = cliente.CalleB;
            Barrio = cliente.Barrio;
            Manzana = cliente.Manzana;
            Piso = cliente.Piso;
            Departamento = cliente.Departamento;
            TelefonoFijo = cliente.TelefonoFijo;
            Celular = cliente.Celular;
            Email = cliente.Email;
            Localidad = new LocalidadViewModel(cliente.Localidad);
        }

        #endregion

        #region Properties

        [DisplayName("Código")]
        public long Id { get; set; }
        [DisplayName("Fecha de Alta")]
        public DateTime FechaAlta { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Apellido { get; set; }
        [Required]
        [StringLength(11, ErrorMessage = "El campo CUIL debe contener 11 caracteres")]
        public string Cuil { get; set; }
        [DisplayName("Fecha de Nacimiento")]
        public DateTime? FechaNacimiento { get; set; }
        [Required]
        public Sexo Sexo { get; set; }
        [Required]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        public string Numero { get; set; }
        [DisplayName("Calle A")]
        public string CalleA { get; set; }
        [DisplayName("Calle B")]
        public string CalleB { get; set; }
        public string Barrio { get; set; }
        public string Manzana { get; set; }
        public string Piso { get; set; }
        [DisplayName("Nº Dpto")]
        public string Departamento { get; set; }
        [DisplayName("Teléfono Fijo")]
        [StringLength(10, ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "TelefonoFijoLength")]
        public string TelefonoFijo { get; set; }
        [StringLength(10, ErrorMessage = "El campo Celular debe contener 11 caracteres")]
        public string Celular { get; set; }
        public string Email { get; set; }

        public LocalidadViewModel Localidad { get; set; }

        public SelectList Provincias { get; set; }
        public SelectList Localidades { get; set; }

        #endregion
    }
}