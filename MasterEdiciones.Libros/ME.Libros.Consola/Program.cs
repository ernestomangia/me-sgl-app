using System;
using System.Linq;
using EFlogger.EntityFramework6;
using LinqKit;

using ME.Libros.Api.Logging;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Logging;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Consola
{
    class Program
    {
        private static ILogger _log;

        static void Main(string[] args)
        {
            EFloggerFor6.Initialize();
            EFloggerFor6.WriteMessage("Text message");

            _log = new Logger();
            _log.Log("La consola se inicio", SeveridadLog.Info);
            Console.WriteLine(" ---- Consola de Pruebas ----");
            var input = Console.ReadLine();
            EstadoVenta? estado = EstadoVenta.Pagada;

            var modelContainer = new ModelContainer();
            var servicio = new VentaService(new EntidadRepository<VentaDominio>(modelContainer));
            var predicateBuilder = PredicateBuilder.True<VentaDominio>();
            if (!string.IsNullOrWhiteSpace(input))
            {
                predicateBuilder = predicateBuilder.And(v => (v.Cliente.Nombre + " " + v.Cliente.Apellido).Contains(input));
            }

            if (estado != null)
            {
                predicateBuilder = predicateBuilder.And(v => v.Estado == estado);
            }

            var ventas = servicio.ListarAsQueryable()
                .AsExpandable()
                .Where(predicateBuilder)
                .ToList();

            foreach (var venta in ventas)
            {
                Console.WriteLine("Nro: " + venta.Id + "| Cliente: " + venta.Cliente.Nombre);
            }

            Console.WriteLine("---- FIN ----");
            Console.ReadLine();
        }
    }
}

