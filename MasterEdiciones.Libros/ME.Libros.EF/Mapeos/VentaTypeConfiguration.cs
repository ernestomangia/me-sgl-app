using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class VentaTypeConfiguration : EntityTypeConfiguration<VentaDominio>
    {
        public VentaTypeConfiguration()
        {
            // PK
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();

            Property(c => c.FechaVenta).IsRequired();
            Property(c => c.FechaCobro).IsRequired();
            
            // FK
            HasRequired(c => c.Cliente);
            //HasRequired(v => v.VentaItems);

            // Map Table
            ToTable("Venta");
        }
    }
}
