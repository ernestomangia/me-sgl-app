using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    class LoginTypeConfiguration: EntityTypeConfiguration<LoginDominio>
    {
        public LoginTypeConfiguration()
        {
            //PK
            HasKey(r => r.Id);

            //Properties
            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.FechaAlta).IsRequired();
            Property(r => r.Usuario).IsRequired().HasMaxLength(30);
            Property(r => r.Contrasena).IsOptional().HasMaxLength(30);

            //Map Table
            ToTable("Login");
        }
    }
}
