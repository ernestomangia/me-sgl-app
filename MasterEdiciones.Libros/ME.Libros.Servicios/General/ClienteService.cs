using System;
using System.Collections.Generic;
using System.Linq;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class ClienteService : AbstractService<ClienteDominio>
    {
        public ClienteService(IRepository<ClienteDominio> repository)
            : base(repository)
        {
        }

        public long GetCodigoCorrelativo()
        {
            try
            {
                return ListarAsQueryable().Max(c => c.Codigo) + 1;
            }
            catch (InvalidOperationException)
            {
                return 1;
            }
        }

        public IEnumerable<ClienteDominio> ListarPorNombre(string query, int limit = 10)
        {
            return ListarAsQueryable()
                .Where(c => (c.Nombre + " " + c.Apellido).Contains(query))
                .OrderBy(c => c.Nombre)
                .ThenBy(c => c.Apellido)
                .Take(limit);
        }
    }
}
