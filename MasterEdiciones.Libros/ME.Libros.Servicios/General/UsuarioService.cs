using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class UsuarioService : AbstractService<UsuarioDominio>
    {
        public UsuarioService(IRepository<UsuarioDominio> repository)
            : base(repository)
        {
            
        }
    }
}
