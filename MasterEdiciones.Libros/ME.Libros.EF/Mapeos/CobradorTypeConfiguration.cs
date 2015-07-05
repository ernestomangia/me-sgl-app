using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

using ME.Libros.Dominio.General;

namespace ME.Libros.EF.Mapeos
{
    public class CobradorTypeConfiguration : EntityTypeConfiguration<CobradorDominio>
    {
        public CobradorTypeConfiguration()
        {
            //PK
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
            Property(c => c.PorcentajeComision).IsRequired();
            Property(c => c.Direccion).HasMaxLength(100);
            Property(c => c.TelefonoFijo);
            Property(c => c.Celular);
            Property(c => c.Email);
            // FK
            HasRequired(c => c.Localidad);
            HasMany(c => c.Localidades)
                .WithMany().Map(c =>
                {
                    c.ToTable("CobradorLocalidades");
                    c.MapLeftKey("Cobrador_Id");
                    c.MapRightKey("Localidad_Id");
                });

            //HasMany(c => c.Localidades).WithRequired().WillCascadeOnDelete(true);

            // Map Table
            ToTable("Cobrador");
        }
    }
}
