using System.ComponentModel;

using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class ClienteViewModel
    {
        #region Properties

        public string Nombre { get; set; }

        public string Apellido { get; set; }

        public string Cuil { get; set; }

        [DisplayName("Fecha de Nacimiento")]
        public string FechaNacimiento { get; set; }

        public Sexo Sexo { get; set; }

        public string Localidad { get; set; }

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

        #endregion
    }
}