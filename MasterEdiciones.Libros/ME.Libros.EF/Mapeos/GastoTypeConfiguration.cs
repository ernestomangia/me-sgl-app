using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    class GastoTypeConfiguration: EntityTypeConfiguration<GastoDominio>
    {
        public GastoTypeConfiguration()
        {
            //PK
            HasKey(r => r.Id);

            //Properties
            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.FechaAlta).IsRequired();
            Property(r => r.Nombre).IsRequired().HasMaxLength(80);
            Property(r => r.Descripcion).IsOptional().HasMaxLength(256);

            //Map Table
            ToTable("Gasto");
        }
    }
}
