using System;
using System.Linq;
using System.Linq.Expressions;

namespace ME.Libros.Api.Repositorios
{
    public interface IRepositorio<T>
    {
        IQueryable<T> Listar();

        IQueryable<T> Listar(Expression<Func<T, bool>> expresion);

        T Get(int id);

        T Get(Expression<Func<T, bool>> expression);

        void Crear(T entidad);

        void Editar(T entidad);

        void Eliminar(T entidad);
    }
}
