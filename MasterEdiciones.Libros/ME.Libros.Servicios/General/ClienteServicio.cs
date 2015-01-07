using ME.Libros.Api.Repositorios;
using Dominio.General;

namespace ME.Libros.Servicios.General
{
    class ClienteServicio : AbstractService<Cliente>
    {
        #region Constructor(s)

        public ClienteServicio(IRepositorio<Cliente> repositorio)
            : base(repositorio)
        {
        }

        #endregion
    }
}
