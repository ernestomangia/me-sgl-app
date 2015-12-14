using System.ComponentModel.DataAnnotations;
using System.Web;

namespace ME.Libros.Web.Models
{
    public class ConfiguracionViewModel
    {
        #region Properties

        [Display(Name = "CarpetaDestino", ResourceType = typeof(Messages))]
        public string CarpetaDestino { get; set; }

        [Display(Name = "BackupFile", ResourceType = typeof(Messages))]
        [Required(ErrorMessageResourceType = typeof(ErrorMessages), ErrorMessageResourceName = "Requerido")]
        public HttpPostedFileBase BackupFile { get; set; }

        #endregion
    }
}