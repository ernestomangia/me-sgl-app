using System;
using System.Linq;
using System.Data.Entity;

using ME.Libros.Api.Repositorios;

namespace ME.Libros.Repositorios
{
    using System.Linq.Expressions;

    public class EntidadRepository<T> : IRepository<T> where T : class
    {
        #region Private Members

        private readonly IModelContainer context;

        #endregion

        #region Costructor(s)

        public EntidadRepository(IModelContainer container)
        {
            context = container;
        }

        #endregion

        #region Public Mehtods

        public IQueryable<T> Listar()
        {
            return context.Set<T>().AsQueryable();
        }

        public IQueryable<T> Listar(Expression<Func<T, bool>> expresion)
        {
            return context.Set<T>().Where(expresion);
        }

        public T Get(int id)
        {
            return context.Set<T>().Find(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return this.context.Set<T>().FirstOrDefault(expression);
        }

        public void Crear(T entidad)
        {
            context.Set<T>().Add(entidad);
            context.SaveChanges();
        }

        public void Editar(T entidad)
        {
            context.Entry(entidad).State = EntityState.Modified;
            context.SaveChanges();
        }

        public void Eliminar(T entidad)
        {
            context.Set<T>().Remove(entidad);
            context.SaveChanges();
        }

        #endregion
    }
}
