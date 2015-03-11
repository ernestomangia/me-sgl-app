using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    using System.Web.Mvc;

    using ME.Libros.Dominio.General;

    public class ClienteViewModel
    {
        #region Constructor(s)

        public ClienteViewModel()
        {
            //Provincias = new SelectList(new List<SelectListItem>
            //{
            //    new SelectListItem
            //    {
            //        Text = "Entre Rios",
            //        Value = "1",
            //    },
            //    new SelectListItem
            //    {
            //        Text = "Santa Fe",
            //        Value = "2",
            //    },
            //    new SelectListItem
            //    {
            //        Text = "San Luis",
            //        Value = "3",
            //    }
            //}, "Value", "Text");
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
            Localidad = cliente.Localidad.Nombre;
            Direccion = cliente.Direccion;
            Departamento = cliente.Departamento;
            TelefonoFijo = cliente.TelefonoFijo;
            Celular = cliente.Celular;
            Email = cliente.Email;
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
        public string Cuil { get; set; }
        [DisplayName("Fecha de Nacimiento")]
        public DateTime? FechaNacimiento { get; set; }
        [Required]
        public Sexo Sexo { get; set; }
        public string Localidad { get; set; }
        [Required]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
        //TODO: Preguntar si esto va
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
        public string TelefonoFijo { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public SelectList Provincias { get; set; }
        [DisplayAttribute(Name = "Provincia")]
        public int ProvinciaId { get; set; }
        public int LocalidadId { get; set; }

        #endregion
    }
}