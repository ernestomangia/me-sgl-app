using System;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using ME.Libros.Api.Logging;
using ME.Libros.Logging;
using Microsoft.SqlServer.Management.Smo;

namespace ME.Libros.Servicios.Configuracion
{
    public class ConfiguracionService
    {
        public SqlConnectionStringBuilder SqlConnection { get; set; }
        private Logger _log;

        public const string BackupName = "sgl.bak";
        private const string TempFolder = "temp";

        public ConfiguracionService()
        {
            _log = new Logger();
            SqlConnection = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["MasterEdicionesDbContext"].ConnectionString);
        }

        public bool CreateBackup(string path)
        {
            var result = true;
            var backup = new Backup
            {
                Action = BackupActionType.Database,
                Database = SqlConnection.InitialCatalog,
                BackupSetDescription = "Full SGL Backup - " + DateTime.Now.ToString("dd/mm/yyyy HH:MM"),
                Incremental = false,
                Initialize = true
            };
            var server = new Server(SqlConnection.DataSource);

            var fileName = BackupName;
            if (!string.IsNullOrEmpty(path))
            {
                if (!Directory.Exists(path))
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                    }
                    catch (Exception ex)
                    {
                        _log.Log("No se pudo crear el directorio para el backup. Ex: " + ex, SeveridadLog.Error);
                        return false;
                    }
                }
                fileName = Path.Combine(path, fileName);
            }

            // Backup
            backup.Devices.AddDevice(fileName, DeviceType.File);

            try
            {
                // Start backup
                backup.SqlBackup(server);
            }
            catch (Exception ex)
            {
                _log.Log("Error al generar el backup de la base de datos. Ex: " + ex, SeveridadLog.Error);
                result = false;
            }

            return result;
        }

        public bool RestoreBackup(HttpPostedFileBase backupFile)
        {
            var result = true;
            // Crear carpeta temporal
            if (!CrearCarpetaTemporal())
            {
                return false;
            }

            // Save temp backup
            var tempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TempFolder, backupFile.FileName);
            backupFile.SaveAs(tempFile);

            // Generate restore
            var restore = new Restore
            {
                Database = SqlConnection.InitialCatalog,
                Action = RestoreActionType.Database,
                ReplaceDatabase = true
            };

            restore.Devices.AddDevice(tempFile, DeviceType.File);

            try
            {
                var server = new Server(SqlConnection.DataSource);

                // Kill all processes
                server.KillAllProcesses(restore.Database);

                // Set single-user mode
                var db = server.Databases[restore.Database];
                db.DatabaseOptions.UserAccess = DatabaseUserAccess.Single;
                db.Alter(TerminationClause.RollbackTransactionsImmediately);

                // Detach database
                server.DetachDatabase(restore.Database, false);

                // Restore backup
                restore.SqlRestore(new Server(SqlConnection.DataSource));
            }
            catch (Exception ex)
            {
                _log.Log("Error al restaurar el backup de la base de datos. Ex:" + ex, SeveridadLog.Error);
                result = false;
            }

            return result;
        }
        public string GetBackupFolder()
        {
            return new Server(SqlConnection.DataSource).BackupDirectory;
        }

        private bool CrearCarpetaTemporal()
        {
            var tempPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, TempFolder);
            if (Directory.Exists(tempPath))
            {
                return true;
            }

            try
            {
                Directory.CreateDirectory(tempPath);
            }
            catch (Exception ex)
            {
                _log.Log("No se pudo crear la carpeta TEMP. Ex: " + ex, SeveridadLog.Error);
                return false;
            }
            return true;
        }
    }
}
