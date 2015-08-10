using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    class IvaTypeConfiguration: EntityTypeConfiguration<IvaDominio>
    {
        public IvaTypeConfiguration()
        {
            //PK
            HasKey(r => r.Id);

            //Properties
            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.FechaAlta).IsRequired();
            Property(r => r.Nombre).IsRequired().HasMaxLength(80);
            Property(r => r.Codigo).IsRequired();
            Property(r => r.Alicuota).IsOptional();
            Property(r => r.HabilitarEliminar).IsRequired();

            //Map Table
            ToTable("Iva");
        }
    }
}
