using System;
using System.Configuration;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.Smo;

namespace ME.Libros.Servicios.Configuracion
{
    public class ConfiguracionService
    {
        public void CreateBackup(string path)
        {
            var connection = new SqlConnectionStringBuilder(ConfigurationManager.ConnectionStrings["MasterEdicionesDbContext"].ConnectionString);
            var backup = new Backup
            {
                Action = BackupActionType.Database
            };
            
            // File name
            var fileName = string.Format("{0}\\{1}", path, "SGL.bak");
            
            // Backup
            backup.Database = connection.InitialCatalog;
            backup.BackupSetDescription = "Full SGL Backup - " + DateTime.Now.ToString("dd/mm/yyyy HH:MM");
            backup.Incremental = false;
            backup.Initialize = true;
            backup.Devices.Add(new BackupDeviceItem(fileName, DeviceType.File));

            // Start backup
            backup.SqlBackup(new Server(connection.DataSource));
        }
    }
}
