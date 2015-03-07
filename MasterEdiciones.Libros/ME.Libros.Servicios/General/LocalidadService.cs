using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class LocalidadService : AbstractService<LocalidadDominio>
    {
        public LocalidadService(IRepository<LocalidadDominio> repository)
            : base(repository)
        {
        }
    }
}
