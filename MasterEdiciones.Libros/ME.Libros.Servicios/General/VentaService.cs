using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class VentaService : AbstractService<VentaDominio>
    {
        public VentaService(IRepository<VentaDominio> repository)
            : base(repository)
        {
        }
    }
}
