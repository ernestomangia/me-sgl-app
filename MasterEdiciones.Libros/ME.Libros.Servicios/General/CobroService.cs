using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        //public CobroService(IRepository<CobroDominio> repository, VentaService ventaService)
        //    : base(repository)
        //{
        //    VentaService = ventaService;
        //}

        public override long Guardar(CobroDominio cobroDominio)
        {
     
           
            if (cobroDominio.EstadoCobro == EstadoCobro.Vigente)
            { cobroDominio.Venta.Saldo = cobroDominio.Venta.Saldo - cobroDominio.Monto; }
            else 
            {
                cobroDominio.Venta.Saldo = cobroDominio.Venta.Saldo + cobroDominio.Monto;
            }



            return base.Guardar(cobroDominio);
        }
    }
}
