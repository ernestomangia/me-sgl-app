﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class ClienteTypeConfiguration : EntityTypeConfiguration<ClienteDominio>
    {
        public ClienteTypeConfiguration()
        {
            // PK
            HasKey(c => c.Id);

            // Properties
            Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(c => c.FechaAlta).IsRequired();
            
            Property(c => c.Nombre).HasMaxLength(100).IsRequired();
            Property(c => c.Apellido).HasMaxLength(100).IsRequired();
            Property(c => c.FechaNacimiento).IsOptional();
            Property(c => c.Cuil).HasMaxLength(11).IsRequired();
            Property(c => c.Direccion).HasMaxLength(200).IsRequired();
            Property(c => c.Numero).IsOptional();
            Property(c => c.Comentario).HasMaxLength(250);
            Property(c => c.Celular).HasMaxLength(11).IsOptional();
            Property(c => c.TelefonoFijo).HasMaxLength(11).IsOptional();
            Property(c => c.Email).HasMaxLength(100).IsOptional();

            // FK
            HasRequired(c => c.Localidad);

            // Map Table
            ToTable("Cliente");
        }
    }
}
