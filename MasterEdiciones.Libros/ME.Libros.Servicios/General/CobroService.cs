using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class CobroService: AbstractService<CobroDominio>
    {
        public CobroService(IRepository<CobroDominio> repository)
            : base(repository)
        {
            
        }
    }
}
