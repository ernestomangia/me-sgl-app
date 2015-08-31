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
            Property(v => v.MontoCalculado).IsRequired();
            Property(v => v.MontoVendido).IsRequired();
            Property(v => v.MontoCobrado).IsRequired();
            Property(v => v.Saldo).IsRequired();
            
            // FK
            HasRequired(v => v.Cliente);
            HasRequired(v => v.Cobrador);
            HasMany(v => v.VentaItems).WithRequired(v => v.Venta);

            // Map Table
            ToTable("Venta");
        }
    }
}
