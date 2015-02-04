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
            if (entidad.Id == 0)
            {
                // Nuevo
                return base.Guardar(DTOADominio(entidad));
            }

            // Modificar
            var cliente = GetPorId(entidad.Id);
            ActualizarDominio(entidad, cliente);
            return base.Guardar(cliente);
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
                },
                TelefonoFijo = clienteDto.TelefonoFijo,
                Celular = clienteDto.Celular,
                Email = clienteDto.Email
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
                Cuil = clienteDominio.Cuil,
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
                TelefonoFijo = clienteDominio.TelefonoFijo,
                Celular = clienteDominio.Celular,
                Email = clienteDominio.Email
            };
        }

        private void ActualizarDominio(ClienteDTO clienteDto, ClienteDominio clienteDominio)
        {
            clienteDominio.Codigo = clienteDto.Codigo;
            clienteDominio.Nombre = clienteDto.Nombre;
            clienteDominio.Apellido = clienteDto.Apellido;
            clienteDominio.Cuil = clienteDto.Cuil;
            clienteDominio.Sexo = clienteDto.Sexo;
            clienteDominio.Direccion = clienteDto.Direccion;
            clienteDominio.Barrio = clienteDto.Barrio;
            clienteDominio.TelefonoFijo = clienteDto.TelefonoFijo;
            clienteDominio.Celular = clienteDto.Celular;
            clienteDominio.Email = clienteDto.Email;
            //clienteDominio.Localidad = new LocalidadDominio
            //{
            //    FechaAlta = DateTime.Now,
            //    Nombre = "Paraná",
            //    Provincia = new ProvinciaDominio
            //    {
            //        FechaAlta = DateTime.Now,
            //        Nombre = "Entre Rios"
            //    }
            //};
        }

        #endregion
    }
}
