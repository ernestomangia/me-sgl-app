using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class CobradorService : AbstractService<CobradorDominio>
    {
        public CobradorService(IRepository<CobradorDominio> repository)
            : base(repository)
        {
        }
    }
}
