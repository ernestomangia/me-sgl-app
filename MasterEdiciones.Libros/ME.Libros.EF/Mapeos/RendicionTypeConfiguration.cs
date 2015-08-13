using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class RendicionTypeConfiguration : EntityTypeConfiguration<RendicionDominio>
    {
        public RendicionTypeConfiguration()
        {
            //PK
            HasKey(r => r.Id);

            // Properties
            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.FechaAlta).IsRequired();

            Property(r => r.Periodo).IsRequired();
            Property(r => r.MontoFacturado).IsRequired();
            Property(r => r.MontoNeto).IsRequired();
            Property(r => r.MontoComision).IsRequired();
            Property(r => r.Comision).IsRequired();

            // FK
            HasRequired(r => r.Localidad);
            HasRequired(r => r.Cobrador);

            ToTable("Rendicion");
        }
    }
}
