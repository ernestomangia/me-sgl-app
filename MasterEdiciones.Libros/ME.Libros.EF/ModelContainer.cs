using System;
using System.Collections.Generic;
using System.Data.Entity;

using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.EF
{
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
                    new ProvinciaDominio {Nombre = "San Luis", FechaAlta = DateTime.Now}
                };

                provincias.ForEach(p => context.Set<ProvinciaDominio>().Add(p));
                context.SaveChanges();
            }
        }
    }
}
