using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Servicios.General
{
    public class CobroService : AbstractService<CobroDominio>
    {
        private VentaService VentaService { get; set; }

        public CobroService(IRepository<CobroDominio> repository)
            : base(repository)
        {

        }

        public override long Guardar(CobroDominio cobroDominio)
        {
            if (cobroDominio.Estado == EstadoCobro.Cobrado)
            {
                cobroDominio.Venta.Saldo = cobroDominio.Venta.Saldo - cobroDominio.Monto;
            }
            else
            {
                cobroDominio.Venta.Saldo = cobroDominio.Venta.Saldo + cobroDominio.Monto;
            }
            
            return base.Guardar(cobroDominio);
        }
    }
}
