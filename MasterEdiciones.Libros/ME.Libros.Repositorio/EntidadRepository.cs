using System;
using System.Linq;
using System.Data.Entity;
using System.Linq.Expressions;

using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio;

namespace ME.Libros.Repositorios
{
    public class EntidadRepository<T> : IRepository<T> where T : BaseDominio
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

        public T Get(long id)
        {
            return context.Set<T>().Find(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return context.Set<T>().FirstOrDefault(expression);
        }

        public long Guardar(T entidad)
        {
            if (entidad.Id == 0)
            {
                context.Set<T>().Add(entidad);
            }
            else
            {
                context.Entry(entidad).State = EntityState.Modified;
            }

            return context.SaveChanges();
        }

        //public int Editar(T entidad)
        //{
        //    context.Entry(entidad).State = EntityState.Modified;
        //    return context.SaveChanges();
        //}

        public void Eliminar(T entidad)
        {
            context.Set<T>().Remove(entidad);
            context.SaveChanges();
        }

        #endregion
    }
}
