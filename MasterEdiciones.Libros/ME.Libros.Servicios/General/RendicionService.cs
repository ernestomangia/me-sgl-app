using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class RendicionService : AbstractService<RendicionDominio>
    {
        public RendicionService(IRepository<RendicionDominio> repository)
            : base(repository)
        {
        }
    }
}
