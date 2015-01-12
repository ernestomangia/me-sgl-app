using System;

using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.Api.Servicios.General;

namespace ME.Libros.Servicios.General
{
    public class ClienteService : AbstractService<ClienteDominio>, IClienteService
    {
        public ClienteService(IRepository<ClienteDominio> repository)
            : base(repository)
        {

        }
    }
}
