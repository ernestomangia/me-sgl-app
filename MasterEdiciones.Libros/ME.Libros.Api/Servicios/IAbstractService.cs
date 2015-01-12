using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace ME.Libros.Api.Servicios
{
    public interface IAbstractService<T>
    {
        void Crear(T entidad);

        void Modificar(T entidad);

        void Eliminar(T entidad);

        T GetPorId(int id);

        T Get(Expression<Func<T, bool>> expression);

        IEnumerable<T> Listar();

        IEnumerable<T> Listar(Expression<Func<T, bool>> expresion);

        IQueryable<T> ListarAsQueryable();

        //Metodos para convertir DTOs
        bool Validar(T entidad);
    }
}
