using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class VentaTypeConfiguration : EntityTypeConfiguration<VentaDominio>
    {
        public VentaTypeConfiguration()
        {
            // PK
            HasKey(v =>v.Id);

            // Properties
            Property(v => v.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(v => v.FechaAlta).IsRequired();
            Property(v => v.FechaVenta).IsRequired();
            Property(v => v.FechaCobro).IsRequired();

            HasMany(v => v.VentaItems).WithRequired(v => v.Venta);
            
            // FK
            HasRequired(v => v.Cliente);
            //HasRequired(v => v.VentaItems);

            // Map Table
            ToTable("Venta");
        }
    }
}
