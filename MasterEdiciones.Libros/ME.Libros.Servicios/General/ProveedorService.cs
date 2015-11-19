using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<ProveedorDominio> ListarPorNombre(string query, int limit = 10)
        {
            return ListarAsQueryable()
                .Where(p => p.RazonSocial.Contains(query))
                .OrderBy(p => p.RazonSocial)
                .Take(limit);
        }
    }
}
