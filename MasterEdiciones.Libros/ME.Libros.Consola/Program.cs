using System;

using ME.Libros.Api.Logging;
using ME.Libros.Logging;
using ME.Libros.Servicios.General;
using ME.Libros.Servicios.DTO;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Consola
{
    class Program
    {
        private static ILogger _log;
        
        static void Main(string[] args)
        {
            _log = new Logger();
            _log.Log("La consola se inicio", SeveridadLog.Info);
            //var modelContainer = new ModelContainer();
            //var clienteServcio = new ClienteService(new EntidadRepository<ClienteDominio>(modelContainer));
            var clienteServcio = new ClienteDTOService();
            clienteServcio.Crear(new ClienteDTO
                                     {
                                         FechaAlta = DateTime.Now,
                                         Codigo = "1000",
                                         Nombre = "Nombre",
                                         Apellido = "apellido",
                                         Cuil = "20364062479",
                                         Barrio = "barrio test",
                                         Direccion = "Direccion",
                                         //Localidad = new LocalidadDominio
                                         //{
                                         //    FechaAlta = DateTime.Now,
                                         //    Nombre = "Paraná"
                                         //},
                                         Sexo = Sexo.Masculino
                                     });
            foreach (var cliente in clienteServcio.Listar())
            {
                Console.WriteLine(cliente.Nombre);
                Console.WriteLine(cliente.Apellido);
                Console.WriteLine(cliente.FechaNacimiento);
            }
            Console.ReadLine();
        }
    }
}
