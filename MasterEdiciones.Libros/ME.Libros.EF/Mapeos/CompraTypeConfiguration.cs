using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class CompraTypeConfiguration:EntityTypeConfiguration<CompraDominio>
    {
        public CompraTypeConfiguration()
        {
            // PK
            HasKey(c =>c.Id);

            // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();
            Property(c => c.FechaCompra).IsRequired();
            Property(c => c.MontoCalculado).IsRequired();
            Property(c => c.MontoComprado).IsRequired();
            Property(c => c.NroFactura).IsRequired();
            Property(c => c.NroRemito).IsRequired();

            // FK
            HasRequired(v => v.Proveedor);
            HasMany(v => v.CompraItems).WithRequired(v => v.Compra);

            // Map Table
            ToTable("Compra");
        }
    }
}
