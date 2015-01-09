using System.Data.Entity;

using ME.Libros.Api.Repositorios;

namespace ME.Libros.EF
{
    public class ModelContainer : DbContext, IModelContainer
    {
        #region Constructor(s)

        public ModelContainer()
            : base("name=MasterEdicionesDbContext")
        {
            Database.SetInitializer<ModelContainer>(new DropCreateDatabaseAlways<ModelContainer>());
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
    }
}
