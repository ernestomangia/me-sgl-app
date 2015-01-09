using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    class ClienteServicio : AbstractService<ClienteDominio>
    {
        #region Constructor(s)

        public ClienteServicio(IRepositorio<ClienteDominio> repositorio)
            : base(repositorio)
        {
        }

        #endregion
    }
}
