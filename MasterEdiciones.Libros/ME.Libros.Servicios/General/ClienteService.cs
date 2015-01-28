using System;
using System.Collections.Generic;
using System.Linq;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.Api.Servicios.General;
using ME.Libros.DTO.General;

namespace ME.Libros.Servicios.General
{
    public class ClienteService : AbstractService<ClienteDominio>, IClienteService
    {
        public ClienteService(IRepository<ClienteDominio> repository)
            : base(repository)
        {

        }
        public virtual int Guardar(ClienteDTO entidad)
        {
            return base.Guardar(DTOADominio(entidad));
        }

        public virtual IEnumerable<ClienteDTO> Listar()
        {
            return base.Listar().Select(DominioADTO);
        }

        #region Private Methods

        private ClienteDominio DTOADominio(ClienteDTO clienteDto)
        {
            return new ClienteDominio
            {
                Id = clienteDto.Id,
                FechaAlta = clienteDto.FechaAlta,
                Codigo = clienteDto.Codigo,
                Nombre = clienteDto.Nombre,
                Apellido = clienteDto.Apellido,
                Cuil = clienteDto.Cuil,
                Sexo = clienteDto.Sexo,
                Direccion = clienteDto.Direccion,
                Barrio = clienteDto.Barrio,
                Localidad = new LocalidadDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Paraná",
                    Provincia = new ProvinciaDominio
                    {
                        FechaAlta = DateTime.Now,
                        Nombre = "Entre Rios"
                    }
                }
            };
        }

        private ClienteDTO DominioADTO(ClienteDominio clienteDominio)
        {
            return new ClienteDTO
            {
                Id = clienteDominio.Id,
                FechaAlta = clienteDominio.FechaAlta,
                Codigo = clienteDominio.Codigo,
                Nombre = clienteDominio.Nombre,
                Apellido = clienteDominio.Apellido,
                Direccion = clienteDominio.Direccion,
                Barrio = clienteDominio.Barrio,
                Localidad = new LocalidadDTO
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Paraná",
                    Provincia = new ProvinciaDTO
                    {
                        FechaAlta = DateTime.Now,
                        Nombre = "Entre Rios"
                    }
                },
            };
        }

        #endregion
    }
}
