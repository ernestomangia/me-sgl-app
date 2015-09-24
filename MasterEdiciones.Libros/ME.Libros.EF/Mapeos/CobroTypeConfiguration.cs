using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class CobroTypeConfiguration : EntityTypeConfiguration<CobroDominio>
    {
        public CobroTypeConfiguration()
        {
            //PK
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();

            Property(c => c.Monto).IsRequired();
            Property(c => c.FechaCobro).IsRequired();
            Property(c => c.Estado).IsRequired();

            HasMany(c => c.Cuotas).WithMany(c => c.Cobros);
            
            // FK
            ToTable("Cobro");
        }
    }
}
