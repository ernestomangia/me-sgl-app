using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ME.Libros.Dominio.General;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.EF;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Servicios.Test
{
    [TestClass]
    public class ClienteTest
    {
        [TestMethod]
        public void ListarClientes()
        {
            var modelContainer = new ModelContainer();
            using (var servicio = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer)))
            {
                var clientes = servicio.Listar().ToList();
                CollectionAssert.AllItemsAreInstancesOfType(clientes, new ClienteDominio().GetType());
            }
        }

        [TestMethod]
        public void AgregarCliente()
        {
            var modelContainer = new ModelContainer();
            using (var servicio = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer)))
            {
                var cliente = new ClienteDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "NombreCliente Test",
                    Apellido = "ApellidoCliente Test",
                    Cuil = "20364062479",
                    Direccion = "Direccion",
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
                    
                 //   Iva = Iva.Exento
                };
                servicio.Guardar(cliente);
                CollectionAssert.Contains(servicio.Listar().ToList(), cliente);
            }
        }
    }
}
