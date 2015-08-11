using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class VentaItemTypeConfiguration : EntityTypeConfiguration<VentaItemDominio>
    {
        public VentaItemTypeConfiguration()
        {
            // PK
            HasKey(v =>v.Id);

            // Properties
            Property(vi => vi.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(vi => vi.FechaAlta).IsRequired();
            Property(vi => vi.Orden).IsRequired();
            Property(vi => vi.Cantidad).IsRequired();
            Property(vi => vi.MontoVendido).IsRequired();
            Property(vi => vi.PrecioCosto).IsRequired();
            Property(vi => vi.PrecioVentaVendido).IsRequired();
            
            // FK
            HasRequired(vi => vi.Venta);
            HasRequired(vi => vi.Producto);

            // Map Table
            ToTable("VentaItem");
        }
    }
}
