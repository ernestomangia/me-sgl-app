using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class ClienteService : AbstractService<ClienteDominio>
    {
        public ClienteService(IRepository<ClienteDominio> repository)
            : base(repository)
        {

        }

    }
}
