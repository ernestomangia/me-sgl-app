using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using ME.Libros.Api.Servicios;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio;

namespace ME.Libros.Servicios
{
    public abstract class AbstractService<T> : IAbstractService<T>, IDisposable where T : BaseDominio 
    {
        #region Private Members

        private readonly IRepository<T> repositorio;
        //private IValidationDictionary validationDictionary;

        #endregion

        #region Constructor(s)

        protected AbstractService(IRepository<T> repositorio)
        {
            this.repositorio = repositorio;
        }

        #endregion

        public virtual int Guardar(T entidad)
        {
            return repositorio.Guardar(entidad);
        }

        //public virtual void Modificar(T entidad)
        //{
        //    repositorio.Editar(entidad);
        //}

        public virtual void Eliminar(T entidad)
        {
            repositorio.Eliminar(entidad);
        }

        public virtual T GetPorId(int id)
        {
            return repositorio.Get(id);
        }

        public virtual T Get(Expression<Func<T, bool>> expression)
        {
            return repositorio.Get(expression);
        }

        public virtual IEnumerable<T> Listar()
        {
            return repositorio.Listar();
        }

        public virtual IEnumerable<T> Listar(Expression<Func<T, bool>> expresion)
        {
            return repositorio.Listar(expresion);
        }

        public virtual IQueryable<T> ListarAsQueryable()
        {
            return repositorio.Listar();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public virtual bool Validar(T entidad)
        {
            return true;
        }
    }
}
