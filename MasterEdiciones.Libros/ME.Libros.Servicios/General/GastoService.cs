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
