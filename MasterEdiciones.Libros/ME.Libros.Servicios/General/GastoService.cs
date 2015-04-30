using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class GastoService : AbstractService<GastoDominio>
    {
        public GastoService(IRepository<GastoDominio> repository)
            : base(repository)
        {
            
        }
    }
}
