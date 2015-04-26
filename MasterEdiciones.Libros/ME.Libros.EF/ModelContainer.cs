using System;
using System.Collections.Generic;
using System.Data.Entity;

using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.EF
{
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class ModelContainer : DbContext, IModelContainer
    {
        #region Constructor(s)

        public ModelContainer()
            : base("name=MasterEdicionesDbContext")
        {
            Database.SetInitializer(new ModelDbInitializer());
        }

        #endregion

        public new IDbSet<TEntidad> Set<TEntidad>() where TEntidad : class
        {
            return base.Set<TEntidad>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(GetType().Assembly);
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            base.OnModelCreating(modelBuilder);
        }

        public class ModelDbInitializer : DropCreateDatabaseIfModelChanges<ModelContainer>
        {
            protected override void Seed(ModelContainer context)
            {
                var provincias = new List<ProvinciaDominio>
                {
                    new ProvinciaDominio {Nombre = "Entre Rios", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Santa Fe", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Córdoba", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "San Luis", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Salta", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "San Juan", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Chaco", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Chubut", FechaAlta = DateTime.Now},
                    new ProvinciaDominio {Nombre = "Santa Cruz", FechaAlta = DateTime.Now},
                };


                var zona = new ZonaDominio
                {
                    Nombre = "Sin definir",
                    Descripcion = "Zona sin definir",
                    FechaAlta = DateTime.Now

                };

                var localidades = new List<LocalidadDominio>
                                      {
                                          new LocalidadDominio {Nombre = "Paraná", Provincia = provincias.First(p => p.Nombre.Equals("Entre Rios"))},
                                          new LocalidadDominio {Nombre = "Crespo", Provincia = provincias.First(p => p.Nombre.Equals("Entre Rios"))},
                                          new LocalidadDominio {Nombre = "Gualeguychú", Provincia = provincias.First(p => p.Nombre.Equals("Entre Rios"))},
                                          new LocalidadDominio {Nombre = "Oro Verde", Provincia = provincias.First(p => p.Nombre.Equals("Entre Rios"))},
                                          new LocalidadDominio {Nombre = "Santa Fe", Provincia = provincias.First(p => p.Nombre.Equals("Santa Fe"))},
                                          new LocalidadDominio {Nombre = "Coronda", Provincia = provincias.First(p => p.Nombre.Equals("Santa Fe"))},
                                          new LocalidadDominio {Nombre = "Venado Tuerto", Provincia = provincias.First(p => p.Nombre.Equals("Santa Fe"))},
                                      };

                var editorial = new EditorialDominio
                {
                    Nombre = "Sin definir",
                    Descripcion = "Editorial sin definir",
                    FechaAlta = DateTime.Now
                };

                var rubro = new RubroDominio
                {
                    Nombre = "Sin definir",
                    Descripcion = "Rubro sin definir",
                    FechaAlta = DateTime.Now
                };
                
                provincias.ForEach(p => context.Set<ProvinciaDominio>().Add(p));
                localidades.ForEach(l =>
                {
                    context.Set<LocalidadDominio>().Add(l);
                    l.FechaAlta = DateTime.Now;
                    l.Zona = zona;
                });
                var cobrador = new CobradorDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Juan",
                    Apellido = "Perez",
                    Cuil = "222222222222"
                };

                var vendedor = new VendedorDominio
                {
                    FechaAlta = DateTime.Now,
                    Nombre = "Juan",
                    Apellido = "Lopez",
                    Cuil = "1111111111"
                };

                context.Set<CobradorDominio>().Add(cobrador);
                context.Set<VendedorDominio>().Add(vendedor);
                context.Set<ZonaDominio>().Add(zona);
                context.Set<EditorialDominio>().Add(editorial);
                context.Set<RubroDominio>().Add(rubro);
                context.SaveChanges();
            }
        }
    }
}
