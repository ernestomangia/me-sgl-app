using ME.Libros.Api.Repositorios;
using Dominio.General;

namespace ME.Libros.Servicios.General
{
    class ClienteServicio : AbstractService<ClienteDominioDominio>
    {
        #region Constructor(s)

        public ClienteServicio(IRepositorio<ClienteDominioDominio> repositorio)
            : base(repositorio)
        {
        }

        #endregion
    }
}
