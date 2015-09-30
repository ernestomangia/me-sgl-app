using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
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
            Property(p => p.MontoCuota).IsRequired();
            Property(p => p.Monto).IsRequired();

            // Map Table
            ToTable("PlanPago");
        }
    }
}
