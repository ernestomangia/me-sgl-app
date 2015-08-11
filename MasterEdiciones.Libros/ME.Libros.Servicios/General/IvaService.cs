using System;
using System.Linq;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class IvaService : AbstractService<IvaDominio>
    {
        public IvaService(IRepository<IvaDominio> repository)
            : base(repository)
        {
            
        }

        public int GetCodigoCorrelativo()
        {
            try
            {
                return ListarAsQueryable().Max(iva => iva.Codigo) + 1;
            }
            catch (InvalidOperationException)
            {
                return 1;
            }
        }
    }
}
