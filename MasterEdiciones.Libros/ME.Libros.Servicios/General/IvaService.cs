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
    }
}
