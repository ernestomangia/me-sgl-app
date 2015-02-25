using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class ProvinciaService : AbstractService<ProvinciaDominio>
    {
        public ProvinciaService(IRepository<ProvinciaDominio> repository)
            : base(repository)
        {
        }
    }
}
