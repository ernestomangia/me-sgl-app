using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    class ZonaTypeConfiguration:EntityTypeConfiguration<ZonaDominio>
    {
        public ZonaTypeConfiguration()
        {
            //PK
            HasKey(z => z.Id);

            //Properties
            Property(z => z.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(z => z.Nombre).HasMaxLength(30).IsRequired();
            Property(z => z.Descripcion).HasMaxLength(250).IsOptional();

            ToTable("Zona");
        }
    }
}
