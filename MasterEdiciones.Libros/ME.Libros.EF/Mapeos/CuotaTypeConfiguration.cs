using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class CuotaTypeConfiguration : EntityTypeConfiguration<CuotaDominio>
    {
        public CuotaTypeConfiguration()
        {
            // PK
            HasKey(c =>c.Id);

            // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();

            Property(c => c.Numero).IsRequired();
            Property(c => c.Estado).IsOptional();
            Property(c => c.FechaVencimiento).IsRequired();
            Property(c => c.FechaCobro).IsOptional();
            Property(c => c.DiasAtraso).IsOptional();
            Property(c => c.Monto).IsRequired();
            Property(c => c.MontoCobro).IsOptional();
            Property(c => c.Saldo).IsOptional();
            Property(c => c.Interes).IsOptional();
            
            // FK
            HasRequired(c => c.Venta);

            // Map Table
            ToTable("Cuota");
        }
    }
}
