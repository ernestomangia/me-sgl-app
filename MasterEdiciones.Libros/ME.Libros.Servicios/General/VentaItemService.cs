using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class VentaItemService : AbstractService<VentaItemDominio>
    {
        public VentaItemService(IRepository<VentaItemDominio> repository)
            : base(repository)
        {
        }
    }
}
