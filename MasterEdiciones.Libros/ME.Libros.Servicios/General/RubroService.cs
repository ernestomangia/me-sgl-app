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
