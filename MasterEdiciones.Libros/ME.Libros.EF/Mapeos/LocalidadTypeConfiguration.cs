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
            this.HasKey(l => l.Id);

            // Properties
            this.Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(l => l.FechaAlta).IsRequired();
            this.Property(l => l.Nombre).HasMaxLength(100).IsRequired();

            this.HasRequired(l => l.Provincia);

            // Map Table
            this.ToTable("Localidad");
            this.Property(l => l.Id).HasColumnName("Id");
            this.Property(l => l.FechaAlta).HasColumnName("Fecha_Alta");
            this.Property(l => l.Nombre).HasColumnName("Nombre");
        }
    }
}
