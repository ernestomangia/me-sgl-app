using System;
using System.Collections.Generic;
using System.Linq;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.DTO;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Servicios.General
{
    public class RendicionService : AbstractService<RendicionDominio>
    {
        private VentaService VentaService { get; set; }

        public RendicionService(IRepository<RendicionDominio> repository)
            : base(repository)
        {
        }

        public RendicionService(IRepository<RendicionDominio> repository, VentaService ventaService)
            : base(repository)
        {
            VentaService = ventaService;
        }

        public void ContabilizarCobros(RendicionDominio rendicionDominio, IEnumerable<CobroDto> cobroDtos)
        {
            foreach (var cobro in cobroDtos)
            {
                var venta = VentaService.GetPorId(cobro.VentaId);
                var montoCobro = cobro.Monto;
                if (montoCobro == 0)
                {
                    //Actualizar cuotas a ATRASADAS
                }
                else
                {
                    var cobroDominio = new CobroDominio
                    {
                        FechaAlta = DateTime.Now,
                        FechaCobro = cobro.FechaCobro,
                        Estado = EstadoCobro.Cobrado,
                        Monto = cobro.Monto,
                        Cuotas = new List<CuotaDominio>()
                    };

                    // Cancelar CUOTAS
                    while (montoCobro > 0)
                    {
                        // Buscar 1ra cuota no pagada
                        var cuota = venta.Cuotas.FirstOrDefault(c => c.Estado != EstadoCuota.Pagada);
                        if (cuota == null)
                        {
                            break;
                        }

                        if (montoCobro >= cuota.Saldo)
                        {
                            // Cuota PAGADA
                            cuota.MontoCobro = cuota.Monto;
                            cuota.Estado = EstadoCuota.Pagada;
                            montoCobro -= cuota.Saldo;
                        }
                        else
                        {
                            // Cuota con saldo deudor. Es PARCIAL. Pero puede ser Atrasada tambien.
                            cuota.MontoCobro += montoCobro;
                            cuota.Estado = EstadoCuota.Parcial;
                            montoCobro = 0;
                        }

                        cuota.Saldo = cuota.Monto - cuota.MontoCobro;
                        cuota.FechaCobro = cobro.FechaCobro;
                        cobroDominio.Cuotas.Add(cuota);
                    }

                    venta.Saldo -= cobro.Monto;
                    rendicionDominio.Cobros.Add(cobroDominio);
                    rendicionDominio.MontoFacturado += cobroDominio.Monto;
                }

                foreach (var cuotaDominio in venta.Cuotas.Where(c => c.Estado != EstadoCuota.Pagada))
                {
                    if (cuotaDominio.FechaVencimiento.Date < DateTime.Now.Date)
                    {
                        cuotaDominio.Estado = EstadoCuota.Atrasada;
                    }
                }
            }

            rendicionDominio.MontoNeto = rendicionDominio.MontoFacturado - rendicionDominio.MontoComision;
        }
    }
}
