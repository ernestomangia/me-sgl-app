using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class ClienteTypeConfiguration : EntityTypeConfiguration<ClienteDominio>
    {
        public ClienteTypeConfiguration()
        {
            // PK
            this.HasKey(c => c.Id);

            // Properties
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(c => c.FechaAlta).IsRequired();
            
            this.Property(c => c.Codigo).HasMaxLength(10).IsOptional(); //remove
            this.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
            this.Property(c => c.Apellido).HasMaxLength(100).IsRequired();
            this.Property(c => c.FechaNacimiento).IsOptional();
            this.Property(c => c.Cuil).HasMaxLength(11).IsRequired();
            this.Property(c => c.Direccion).HasMaxLength(100).IsRequired();
            this.Property(c => c.Barrio).HasMaxLength(100).IsOptional();
            this.Property(c => c.CalleA).HasMaxLength(100).IsOptional();
            this.Property(c => c.CalleB).HasMaxLength(100).IsOptional();
            this.Property(c => c.Piso).HasMaxLength(2).IsOptional();
            this.Property(c => c.Departamento).HasMaxLength(2).IsOptional();
            this.Property(c => c.Manzana).HasMaxLength(2).IsOptional();
            this.Property(c => c.Celular).HasMaxLength(11).IsOptional();
            this.Property(c => c.TelefonoFijo).HasMaxLength(11).IsOptional();
            this.Property(c => c.Email).HasMaxLength(100).IsOptional();

            // FK
            this.HasRequired(c => c.Localidad);

            // Map Table
            this.ToTable("Cliente");
            this.Property(c => c.Id).HasColumnName("Id");
            this.Property(c => c.FechaAlta).HasColumnName("Fecha_Alta");

            this.Property(c => c.Codigo).HasColumnName("Codigo");
            this.Property(c => c.Nombre).HasColumnName("Nombre");
            this.Property(c => c.Apellido).HasColumnName("Apellido");
            this.Property(c => c.FechaNacimiento).HasColumnName("Fecha_Nacimiento");
            this.Property(c => c.Cuil).HasColumnName("Cuil");
            this.Property(c => c.Direccion).HasColumnName("Direccion");
            this.Property(c => c.Barrio).HasColumnName("Barrio");
            this.Property(c => c.CalleA).HasColumnName("CalleA");
            this.Property(c => c.CalleB).HasColumnName("CalleB");
            this.Property(c => c.Piso).HasColumnName("Piso");
            this.Property(c => c.Departamento).HasColumnName("Departamento");
            this.Property(c => c.Manzana).HasColumnName("Manzana");
            this.Property(c => c.Celular).HasColumnName("Celular");
            this.Property(c => c.TelefonoFijo).HasColumnName("Telefono_Fijo");
            this.Property(c => c.Email).HasColumnName("Email");
        }
    }
}
