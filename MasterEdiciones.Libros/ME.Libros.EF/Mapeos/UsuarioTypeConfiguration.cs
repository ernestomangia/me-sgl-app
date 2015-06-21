using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    class UsuarioTypeConfiguration: EntityTypeConfiguration<UsuarioDominio>
    {
        public UsuarioTypeConfiguration()
        {
            //PK
            HasKey(r => r.Id);

            //Properties
            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.FechaAlta).IsRequired();
            Property(r => r.Nombre).IsRequired().HasMaxLength(80);
            Property(r => r.Contrasena).IsOptional().HasMaxLength(256);

            //Map Table
            ToTable("Usuario");
        }
    }
}
