using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class ProvinciaTypeConfiguration : EntityTypeConfiguration<ProvinciaDominio>
    {
        public ProvinciaTypeConfiguration()
        {
            // PK
            this.HasKey(p => p.Id);

            // Properties
            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(p => p.FechaAlta).IsRequired();
            this.Property(p => p.Nombre).HasMaxLength(100).IsRequired();

            // Map Table
            this.ToTable("Provincia");
            this.Property(p => p.Id).HasColumnName("Id");
            this.Property(p => p.FechaAlta).HasColumnName("Fecha_Alta");
            this.Property(p => p.Nombre).HasColumnName("Nombre");
        }
    }
}
