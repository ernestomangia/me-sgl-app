using System;
using System.Linq;
using System.Collections.Generic;

using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.DTO.General;
using ME.Libros.Repositorios;
using ME.Libros.EF;

namespace ME.Libros.Servicios.General
{
    public class ClienteDTOService : IDisposable
    {
        private EntidadRepository<ClienteDominio> _repository;
        private IModelContainer modelContainer;

        #region Constructor(s)

        public ClienteDTOService()
        {
            modelContainer = new ModelContainer();
            _repository = new EntidadRepository<ClienteDominio>(modelContainer);
        }


        public void Crear(ClienteDTO clienteDto)
        {
            _repository.Guardar(DtoADominio(clienteDto));
        }

        public IEnumerable<ClienteDTO> Listar()
        {
            var clientes = new List<ClienteDTO>();
            _repository.Listar().ToList().ForEach(c => clientes.Add(DominioADto(c)));
            return clientes;
        }

        private ClienteDominio DtoADominio(ClienteDTO clienteDto)
        {
            return new ClienteDominio
                       {
                           Id = clienteDto.Id,
                           FechaAlta = clienteDto.FechaAlta,
                           Nombre = clienteDto.Nombre,
                           Apellido = clienteDto.Apellido,
                           Cuil = clienteDto.Cuil,
                           Sexo = clienteDto.Sexo,
                           Barrio = clienteDto.Barrio,
                       };
        }

        private ClienteDTO DominioADto(ClienteDominio clienteDominio)
        {
            return new ClienteDTO
            {
                Id = clienteDominio.Id,
                FechaAlta = clienteDominio.FechaAlta,
                Nombre = clienteDominio.Nombre,
                Apellido = clienteDominio.Apellido,
                Barrio = clienteDominio.Barrio,
            };
        }

        #endregion

        public void Dispose()
        {
            //GC.SuppressFinalize(this);
        }
    }
}
