using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class VendedorService : AbstractService<VendedorDominio>
    {
        public VendedorService(IRepository<VendedorDominio> repository)
            : base(repository)
        {
        }
    }
}
