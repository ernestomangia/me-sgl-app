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

        private readonly IModelContainer _context;

        #endregion

        #region Costructor(s)

        public EntidadRepository(IModelContainer container)
        {
            _context = container;
        }

        #endregion

        #region Public Mehtods

        public IQueryable<T> Listar()
        {
            return _context.Set<T>().AsQueryable();
        }

        public IQueryable<T> Listar(Expression<Func<T, bool>> expresion)
        {
            return _context.Set<T>().Where(expresion);
        }

        public T Get(long id)
        {
            return _context.Set<T>().Find(id);
        }

        public T Get(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().FirstOrDefault(expression);
        }

        public long Guardar(T entidad)
        {
            if (entidad.Id == 0)
            {
                _context.Set<T>().Add(entidad);
            }
            else
            {
                _context.Entry(entidad).State = EntityState.Modified;
            }

            return _context.SaveChanges();
        }

        //public int Editar(T entidad)
        //{
        //    context.Entry(entidad).State = EntityState.Modified;
        //    return context.SaveChanges();
        //}

        public void Eliminar(T entidad)
        {
            _context.Set<T>().Remove(entidad);
            _context.SaveChanges();
        }

        #endregion
    }
}
