using System.Data.Entity.ModelConfiguration;

using Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class ClienteTypeConfiguration : EntityTypeConfiguration<ClienteDominioDominio>
    {
        public ClienteTypeConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            this.Property(c => c.FechaAlta);

            this.Property(c => c.Codigo).IsRequired();
            this.Property(c => c.Nombre).HasMaxLength(80).IsRequired();
            this.Property(c => c.Apellido).HasMaxLength(80).IsRequired();
            this.Property(c => c.Cuil).HasMaxLength(11).IsRequired();

            this.ToTable("Cliente");
            this.Property(c => c.Id).HasColumnName("Id");
            this.Property(c => c.FechaAlta).HasColumnName("Fecha_Alta");
            this.Property(c => c.Codigo).HasColumnName("Codigo");
            this.Property(c => c.Nombre).HasColumnName("Nombre");
            this.Property(c => c.Apellido).HasColumnName("Apellido");
            this.Property(c => c.FechaNacimiento).HasColumnName("Fecha_Nacimiento"); ;
            this.Property(c => c.Cuil).HasColumnName("Cuil");;
        }
    }
}
