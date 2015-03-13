using System.Collections.Generic;

namespace ME.Libros.Web.Models
{
    using System.Collections;

    public class ResultadoViewModel
    {
        public ResultadoViewModel()
        {
            Success = false;
            Messages = new Dictionary<string, string>();
        }

        public bool Success { get; set; }
        public IDictionary Messages { get; set; }

    }
}