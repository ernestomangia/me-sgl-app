﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class CompraItemTypeConfiguration:EntityTypeConfiguration<CompraItemDominio>
    {
        public CompraItemTypeConfiguration()
        {
            // PK
            HasKey(ci => ci.Id);

            // Properties
            Property(ci => ci.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(ci => ci.FechaAlta).IsRequired();
            Property(vi => vi.Orden).IsRequired();
            Property(ci => ci.Cantidad).IsRequired();
            Property(ci => ci.MontoComprado).IsRequired();
            Property(ci => ci.PrecioCostoAnterior).IsRequired();
            Property(ci => ci.PrecioCostoComprado).IsRequired();

            // FK
            HasRequired(vi => vi.Compra);
            HasRequired(vi => vi.Producto);

            // Map Table
            ToTable("CompraItem");
        }
    }
}
