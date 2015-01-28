using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using ME.Libros.DTO.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
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

        public ClienteViewModel(ClienteDTO clienteDto)
        {
            Nombre = clienteDto.Nombre;
            Apellido = clienteDto.Apellido;
            Cuil = clienteDto.Cuil;
            Direccion = clienteDto.Direccion;
        }

        #endregion

        #region Properties

        public int Id { get; set; }

        [DisplayName("Fecha de Alta")]
        public DateTime FechaAlta { get; set; }

        [Required]
        [DisplayName("Código")]
        public string Codigo { get; set; }

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

        [DisplayName("Calle A")]
        public string CalleA { get; set; }

        [DisplayName("Calle B")]
        public string CalleB { get; set; }

        public string Barrio { get; set; }

        public string Manzana { get; set; }

        public string Piso { get; set; }

        [DisplayName("Número")]
        public string Numero { get; set; }

        [DisplayName("Teléfono Fijo")]
        public string TelefonoFijo { get; set; }

        public string Celular { get; set; }

        public string Email { get; set; }

        //public SelectList Provincias { get; set; }

        #endregion
    }
}