
using System.ComponentModel.DataAnnotations;

namespace ME.Libros.Web.Models
{
    public class ConfiguracionViewModel
    {
        #region Properties

        [Display(Name = "Ruta", ResourceType = typeof(Messages))]
        public string Ruta { get; set; }

        #endregion
    }
}