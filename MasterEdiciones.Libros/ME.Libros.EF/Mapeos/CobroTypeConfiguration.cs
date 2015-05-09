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
    class CobroTypeConfiguration:EntityTypeConfiguration<CobroDominio>
    {
        public CobroTypeConfiguration()
        {

            //PK
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();

            Property(c => c.Monto).IsRequired();
            Property(c => c.FechaCobro).IsRequired();
            Property(c => c.EstadoCobro).IsRequired();
            Property(c => c.Comision).IsRequired();


            // FK
            HasRequired(c => c.Venta);
            HasRequired(c => c.Cobrador);

            ToTable("Cobro");
        }
    }
}
