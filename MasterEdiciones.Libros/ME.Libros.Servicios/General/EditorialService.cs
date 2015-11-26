using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class EditorialService:AbstractService<EditorialDominio>
    {
         public EditorialService(IRepository<EditorialDominio> repository)
            : base(repository)
        {
            
        }
    }
}
