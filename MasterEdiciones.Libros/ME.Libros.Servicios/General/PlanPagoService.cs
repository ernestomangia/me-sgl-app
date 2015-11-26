using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class PlanPagoService : AbstractService<PlanPagoDominio>
    {
        public PlanPagoService(IRepository<PlanPagoDominio> repository)
            : base(repository)
        {
        }
    }
}
