using System;

using ME.Libros.Api.Logging;
using ME.Libros.Logging;

namespace ME.Libros.Consola
{
    class Program
    {
        private static ILogger _log;
        
        static void Main(string[] args)
        {
            _log = new Logger();
            _log.Log("La consola se inicio", SeveridadLog.Info);
            
            Console.ReadLine();
        }
    }
}
