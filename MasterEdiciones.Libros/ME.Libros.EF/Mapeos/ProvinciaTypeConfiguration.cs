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
            HasKey(p => p.Id);

            // Properties
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.FechaAlta).IsRequired();
            Property(p => p.Nombre).HasMaxLength(100).IsRequired();

            // Map Table
            ToTable("Provincia");
        }
    }
}
