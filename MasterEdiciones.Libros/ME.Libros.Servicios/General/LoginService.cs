using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class LoginService : AbstractService<LoginDominio>
    {
        public LoginService(IRepository<LoginDominio> repository)
            : base(repository)
        {
            
        }
    }
}
