using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    class LocalidadTypeConfiguration : EntityTypeConfiguration<LocalidadDominio>
    {
        public LocalidadTypeConfiguration()
        {
            // PK
            this.HasKey(c => c.Id);

            // Properties
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(c => c.FechaAlta).IsRequired();
            this.Property(c => c.Nombre).HasMaxLength(100).IsRequired();

            // Map Table
            this.ToTable("Localidad");
            this.Property(c => c.Id).HasColumnName("Id");
            this.Property(c => c.FechaAlta).HasColumnName("Fecha_Alta");
            this.Property(c => c.Nombre).HasColumnName("Nombre");
        }
    }
}
