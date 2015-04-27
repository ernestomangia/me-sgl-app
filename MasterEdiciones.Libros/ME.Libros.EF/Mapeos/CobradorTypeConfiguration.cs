﻿using System;
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
    public class CobradorTypeConfiguration: EntityTypeConfiguration<CobradorDominio>
    {
        public CobradorTypeConfiguration()
        { //PK
            HasKey(c => c.Id);


          // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();

            Property(c => c.Nombre).HasMaxLength(100).IsRequired();
            Property(c => c.Apellido).HasMaxLength(100).IsRequired();
            Property(c => c.Dni).HasMaxLength(8)
                .IsRequired()
                .HasColumnAnnotation("Index",
                    new IndexAnnotation(new[]
                    {
                        new IndexAttribute("Index") {IsUnique = true}
                    }));

            // FK
            HasRequired(c => c.Localidad);
            HasMany(c => c.Localidades)
                .WithMany().Map(c =>
                {  
                    c.ToTable("CobradorLocalidades");
                    c.MapLeftKey("CobradorId");
                    c.MapRightKey("LocalidadId");
                });
                
            
            
            
            //HasRequired(c => c.Localidades).WithOptional();




        ToTable("Cobrador");
        }

        }
}