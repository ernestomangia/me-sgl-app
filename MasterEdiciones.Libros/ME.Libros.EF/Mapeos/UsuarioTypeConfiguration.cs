using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    class UsuarioTypeConfiguration: EntityTypeConfiguration<UsuarioDominio>
    {
        public UsuarioTypeConfiguration()
        {
            //PK
            HasKey(r => r.Id);

            //Properties
            Property(r => r.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(r => r.FechaAlta).IsRequired();

            Property(r => r.Nombre).HasMaxLength(100).IsRequired();
            Property(r => r.Apellido).HasMaxLength(100).IsRequired();
            Property(r => r.UserName).HasMaxLength(50).IsRequired();
            Property(r => r.Password).HasMaxLength(50).IsRequired();
            Property(r => r.Email).HasMaxLength(100).IsOptional();
            Property(r => r.EmailConfirmado).IsRequired();
            Property(r => r.Habilitado).IsRequired();
            Property(r => r.UltimoLogin).IsOptional();
            Property(r => r.CantidadIntentosFallidos).IsRequired();

            //Map Table
            ToTable("Usuario");
        }
    }
}
