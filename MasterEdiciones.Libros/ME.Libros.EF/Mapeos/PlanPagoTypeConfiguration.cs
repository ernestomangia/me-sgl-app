using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class PlanPagoTypeConfiguration:EntityTypeConfiguration<PlanPagoDominio>
    {
        public PlanPagoTypeConfiguration()
        {
            // PK
            HasKey(p => p.Id);

            // Properties
            Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(p => p.FechaAlta).IsRequired();

            Property(p => p.Nombre).HasMaxLength(100).IsRequired();
            Property(p => p.Descripcion).HasMaxLength(150).IsOptional();
            Property(p => p.CantidadCuotas).IsRequired();
            Property(p => p.Tipo).IsRequired();
            Property(p => p.Monto).IsRequired();

            // Map Table
            ToTable("PlanPago");
        }
    }
}
