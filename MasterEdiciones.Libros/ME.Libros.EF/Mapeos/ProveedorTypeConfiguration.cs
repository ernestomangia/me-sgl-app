using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class ProveedorTypeConfiguration : EntityTypeConfiguration<ProveedorDominio>
    {
        public ProveedorTypeConfiguration()
        {
            // PK
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();
            
            Property(c => c.RazonSocial).HasMaxLength(100).IsRequired();
            Property(c => c.Cuil).HasMaxLength(13)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute { IsUnique = true }));
            Property(c => c.Direccion).HasMaxLength(200).IsOptional();
            Property(c => c.Celular).HasMaxLength(11).IsOptional();
            Property(c => c.TelefonoFijo).HasMaxLength(11).IsOptional();
            Property(c => c.Email).HasMaxLength(100).IsOptional();

            // FK
            HasRequired(c => c.Localidad);

            // Map Table
            ToTable("Proveedor");
        }
    }
}
