using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace ME.Libros.Api.Servicios
{
    public interface IAbstractService<T>
    {
        int Guardar(T entidad);
        void Eliminar(T entidad);
        T GetPorId(int id);
        T Get(Expression<Func<T, bool>> expression);
        IEnumerable<T> Listar();
        IEnumerable<T> Listar(Expression<Func<T, bool>> expresion);
        IQueryable<T> ListarAsQueryable();
        bool Validar(T entidad);
    }
}
