using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace ME.Libros.Api.Repositorios
{
    public interface IModelContainer : IDisposable
    {
        IDbSet<TEntidad> Set<TEntidad>() where TEntidad : class;

        int SaveChanges();

        DbEntityEntry<TEntidad> Entry<TEntidad>(TEntidad entidad) where TEntidad : class;
    }
}
