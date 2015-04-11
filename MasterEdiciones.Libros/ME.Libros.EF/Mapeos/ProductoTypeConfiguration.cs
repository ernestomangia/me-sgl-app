using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Dominio;
using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class ProductoTypeConfiguration: EntityTypeConfiguration<ProductoDominio>
    {
        public ProductoTypeConfiguration()
        {
            //PK
            HasKey(p => p.Id);

            //Properties
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.FechaAlta).IsRequired();

            Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            Property(p => p.Descripcion).HasMaxLength(250).IsOptional();
            Property(p => p.Stock).IsOptional();
            Property(p => p.CodigoBarra).HasMaxLength(30);
            Property(p => p.PrecioCosto).IsRequired();
            Property(p => p.PrecioVenta).IsRequired();

            //FK
            HasRequired(p => p.Editorial);
            HasRequired(p => p.Rubro);


            //Map Table
            ToTable("Producto");
        }
    }
}
