using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class GastoVendedorTypeConfiguration : EntityTypeConfiguration<GastoVendedorDominio>
    {
        public GastoVendedorTypeConfiguration()
        {
            // PK
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();

            Property(c => c.FechaGasto).IsRequired();
            Property(c => c.Monto).IsRequired();

            // FK
            HasRequired(c => c.Vendedor);
            HasRequired(c => c.Gasto);

            // Map Table
            ToTable("GastoVendedorDominio");
        }
    }
}
