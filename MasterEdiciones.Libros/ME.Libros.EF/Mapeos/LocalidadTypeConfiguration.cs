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
            HasKey(l => l.Id);

            // Properties
            Property(l => l.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(l => l.FechaAlta).IsRequired();
            Property(l => l.Nombre).HasMaxLength(100).IsRequired();

            HasRequired(l => l.Provincia);
            HasRequired(l => l.Zona);

            // Map Table
            ToTable("Localidad");
        }
    }
}
