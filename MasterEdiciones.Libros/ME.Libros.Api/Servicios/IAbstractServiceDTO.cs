using System.Collections.Generic;

namespace ME.Libros.Api
{
    public interface IAbstractServiceDTO<T>
    {
        int Guardar(T entidad);

        //void Eliminar(T entidad);

        //T GetPorId(int id);

        //T Get(Expression<Func<T, bool>> expression);

        IEnumerable<T> Listar();

        //IEnumerable<T> Listar(Expression<Func<T, bool>> expresion);

        //IQueryable<T> ListarAsQueryable();

        ////Metodos para convertir DTOs
        //bool Validar(T entidad);
    }
}
