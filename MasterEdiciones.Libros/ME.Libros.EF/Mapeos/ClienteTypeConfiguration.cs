using System.Data.Entity.ModelConfiguration;
using Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class ClienteTypeConfiguration : EntityTypeConfiguration<Cliente>
    {
        public ClienteTypeConfiguration()
        {
            this.HasKey(c => c.Id);
            this.Property(c => c.Nombre).HasMaxLength(80).IsRequired();
            this.Property(c => c.Apellido).HasMaxLength(80).IsRequired();
            this.Property(c => c.FechaNacimiento);
            this.Property(c => c.Cuil).HasMaxLength(11).IsRequired();

            this.ToTable("Cliente");
        }
    }
}
