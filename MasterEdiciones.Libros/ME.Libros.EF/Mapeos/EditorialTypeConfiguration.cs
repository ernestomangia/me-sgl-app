using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    class EditorialTypeConfiguration: EntityTypeConfiguration<EditorialDominio>
    {
        public EditorialTypeConfiguration()
        { //PK
            HasKey(e => e.Id);

            //Properties
            Property(e => e.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(e => e.Nombre).IsRequired().HasMaxLength(80);
            Property(e => e.Descripcion).IsOptional().HasMaxLength(256);
            Property(e => e.FechaAlta).IsRequired();

            //Map Table
            ToTable("Editorial");
        }
    }
}
