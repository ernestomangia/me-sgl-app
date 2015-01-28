using ME.Libros.Dominio.General;
using ME.Libros.DTO.General;

namespace ME.Libros.Api.Servicios.General
{
    public interface IClienteService : IAbstractService<ClienteDominio>, IAbstractServiceDTO<ClienteDTO>
    {

    }
}
