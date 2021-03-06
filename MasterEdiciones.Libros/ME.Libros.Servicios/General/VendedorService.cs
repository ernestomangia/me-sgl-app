﻿using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<VendedorDominio> ListarPorNombre(string query, int limit = 10)
        {
            return ListarAsQueryable()
                .Where(c => (c.Nombre + " " + c.Apellido).Contains(query))
                .OrderBy(c => c.Nombre)
                .ThenBy(c => c.Apellido)
                .Take(limit);
        }
    }
}
