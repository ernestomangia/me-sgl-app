using System;
using ME.Libros.Utils.Enums;

namespace ME.Libros.DTO.General
{
    public class ClienteDTO : BaseDTO
    {
        #region Properties

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Cuil { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public Sexo Sexo { get; set; }
        public LocalidadDTO Localidad { get; set; }
        public string Direccion { get; set; }
        public string CalleA { get; set; }
        public string CalleB { get; set; }
        public string Barrio { get; set; }
        public string Manzana { get; set; }
        public string Piso { get; set; }
        public string Numero { get; set; }
        public string TelefonoFijo { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }

        #endregion
    }
}
