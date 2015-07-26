using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class CompraItemService : AbstractService<CompraItemDominio>
    {
        public CompraItemService(IRepository<CompraItemDominio> repository)
            : base(repository)
        {
        }
    }
}