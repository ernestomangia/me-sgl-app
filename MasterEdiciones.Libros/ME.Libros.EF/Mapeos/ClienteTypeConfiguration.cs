using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class ClienteTypeConfiguration : EntityTypeConfiguration<ClienteDominio>
    {
        public ClienteTypeConfiguration()
        {
            // PK
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();
            
            Property(c => c.Nombre).HasMaxLength(100).IsRequired();
            Property(c => c.Apellido).HasMaxLength(100).IsRequired();
            Property(c => c.FechaNacimiento).IsOptional();
            Property(c => c.Cuil).HasMaxLength(11).IsRequired();
            Property(c => c.Direccion).HasMaxLength(100).IsRequired();
            Property(c => c.Barrio).HasMaxLength(100).IsOptional();
            Property(c => c.CalleA).HasMaxLength(100).IsOptional();
            Property(c => c.CalleB).HasMaxLength(100).IsOptional();
            Property(c => c.Piso).HasMaxLength(2).IsOptional();
            Property(c => c.Departamento).HasMaxLength(2).IsOptional();
            Property(c => c.Manzana).HasMaxLength(2).IsOptional();
            Property(c => c.Celular).HasMaxLength(11).IsOptional();
            Property(c => c.TelefonoFijo).HasMaxLength(11).IsOptional();
            Property(c => c.Email).HasMaxLength(100).IsOptional();

            // FK
            HasRequired(c => c.Localidad);

            // Map Table
            ToTable("Cliente");
        }
    }
}
