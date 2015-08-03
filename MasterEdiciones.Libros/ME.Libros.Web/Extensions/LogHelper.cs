using ME.Libros.Api.Logging;
using ME.Libros.Logging;

namespace ME.Libros.Web.Extensions
{
    public class LogHelper
    {
        public static void Log(string message, SeveridadLog severidad)
        {
            // TODO: Ver como hacer un Singleton para no instanciar el logger cada vez que se loguea
            var logger = new Logger();
            logger.Log(message, severidad);
        }
    }
}