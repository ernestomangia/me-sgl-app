using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class RubroService:AbstractService<RubroDominio>
    {
        public RubroService(IRepository<RubroDominio> repository)
            : base(repository)
        {
            
        }
    }
}
