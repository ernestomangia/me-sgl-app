using System.Configuration;
using System.IO;
using System.Reflection;
using log4net;

using ME.Libros.Api.Logging;

namespace ME.Libros.Logging
{
    public class Logger : ILogger
    {
        #region Constructor(s)

        /// <summary>
        /// Initializes a new instance of the <see cref="Logger"/> class.
        /// </summary>
        public Logger()
        {
            var path = ConfigurationManager.AppSettings["log4netConfig"];
            log4net.Config.XmlConfigurator.ConfigureAndWatch(new FileInfo(path));
        }

        #endregion

        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Log(string mensaje, SeveridadLog severidad)
        {
            switch (severidad)
            {
                case SeveridadLog.Debug:
                    _log.Debug(mensaje);
                    break;
                case SeveridadLog.Info:
                    _log.Info(mensaje);
                    break;
                case SeveridadLog.Warning:
                    _log.Warn(mensaje);
                    break;
                case SeveridadLog.Error:
                    _log.Error(mensaje);
                    break;
            }
        }
    }
}
