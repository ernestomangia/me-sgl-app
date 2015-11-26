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
    class RubroTypeConfiguration: EntityTypeConfiguration<RubroDominio>
    {
        public RubroTypeConfiguration()
        {
            //PK
            HasKey(r => r.Id);

            //Properties
            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.Nombre).IsRequired().HasMaxLength(80);
            Property(r => r.Descripcion).IsOptional().HasMaxLength(250);
            Property(r => r.FechaAlta).IsRequired();

            //Map Table
            ToTable("Rubro");
        }
    }
}
