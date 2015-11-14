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

        private readonly IRepository<T> _repositorio;

        public Dictionary<string, string> ModelError;

        #endregion

        #region Constructor(s)

        protected AbstractService(IRepository<T> repositorio)
        {
            this._repositorio = repositorio;
            ModelError = new Dictionary<string, string>();
        }

        #endregion

        public virtual long Guardar(T entidad)
        {
            return _repositorio.Guardar(entidad);
        }

        public virtual void Eliminar(T entidad)
        {
            _repositorio.Eliminar(entidad);
        }

        public virtual T GetPorId(long id)
        {
            return _repositorio.Get(id);
        }

        public virtual T Get(Expression<Func<T, bool>> expression)
        {
            return _repositorio.Get(expression);
        }

        public virtual IEnumerable<T> Listar()
        {
            return _repositorio.Listar();
        }

        public virtual IEnumerable<T> Listar(Expression<Func<T, bool>> expresion)
        {
            return _repositorio.Listar(expresion);
        }

        public virtual IQueryable<T> ListarAsQueryable()
        {
            return _repositorio.Listar();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public virtual bool Validar(T entidad)
        {
            return ModelError.Count == 0;
        }
    }
}
