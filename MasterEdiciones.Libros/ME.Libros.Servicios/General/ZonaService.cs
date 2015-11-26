using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class ZonaService:AbstractService<ZonaDominio>
    {
        public ZonaService(IRepository<ZonaDominio> repository)
            : base(repository)
        {
            
        }
    }
}
