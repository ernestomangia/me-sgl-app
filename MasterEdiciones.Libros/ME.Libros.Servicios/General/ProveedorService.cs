using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class ProveedorService : AbstractService<ProveedorDominio>
    {
        public ProveedorService(IRepository<ProveedorDominio> repository)
            : base(repository)
        {
        }
    }
}
