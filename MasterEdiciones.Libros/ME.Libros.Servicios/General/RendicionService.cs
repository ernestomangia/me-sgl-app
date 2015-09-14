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
                if (montoCobro > 0)
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
                            // El monto cobrado supera al saldo de la venta
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
                            // Cuota con saldo deudor, queda PARCIAL (si esta vencida va a quedar ATRASADA)
                            cuota.MontoCobro += montoCobro;
                            cuota.Estado = EstadoCuota.Parcial;
                            montoCobro = 0;
                        }

                        cuota.Saldo = cuota.Monto - cuota.MontoCobro;
                        cuota.FechaCobro = cobro.FechaCobro;
                        cobroDominio.Cuotas.Add(cuota);
                    }

                    VentaService.ContabilizarCobro(venta, cobroDominio.Monto);
                    rendicionDominio.Cobros.Add(cobroDominio);
                }

                VentaService.ActualizarEstadoCuotas(venta);
                VentaService.ActualizarEstadoVenta(venta);
            }

            CalcularMontos(rendicionDominio);
        }

        public void ModificarRendicion(RendicionDominio rendicionDominio, List<CobroDto> cobroDtos)
        {
            // cobroDtos: nuevos montos de cobro
            // rendicionDominio.Cobros: cobros anteriores
            foreach (var cobro in rendicionDominio.Cobros)
            {
                // Buscar el cobro modificado
                var cobroDto = cobroDtos.Single(c => c.Id == cobro.Id);
                if (cobro.Monto != cobroDto.Monto)
                {
                    var venta = cobro.Cuotas.First().Venta;
                    var cobrosVenta = venta.Cuotas.SelectMany(c => c.Cobros).Distinct().ToList();

                    // Saldo de la 1er cuota del cobro a modificar
                    var saldoCuota = cobrosVenta.Where(cob => cob.Id < cobro.Id).Sum(cob => cob.Monto) -
                                     venta.Cuotas.Where(cuota => cuota.Numero <= cobro.Cuotas.First().Numero).Sum(cuota => cuota.Monto);

                    // Actualizar saldo de la venta con la diferencia entre el nuevo monto cobrado y el monto modificado
                    VentaService.ContabilizarCobro(venta, cobroDto.Monto - cobro.Monto);

                    // El monto se modfico
                    if (cobroDto.Monto == 0)
                    {
                        // Eliminar cobro
                        rendicionDominio.Cobros.Remove(cobro);
                    }
                    else
                    {
                        cobro.Monto = cobroDto.Monto;
                    }
                    
                    // Recalcular saldo cuotas y relaciones
                    RecalcularCuotas(venta, cobrosVenta, cobro, saldoCuota);
                }
            }

            CalcularMontos(rendicionDominio);
        }

        public void RecalcularCuotas(VentaDominio venta, List<CobroDominio> cobrosVenta, CobroDominio cobroModificado, decimal saldoCuota)
        {
            var cuotas = venta.Cuotas.Where(cuota => cuota.Id >= cobroModificado.Cuotas.First().Id).ToList();
            foreach (var cuota in cuotas)
            {
                cuota.Saldo = cuota.Monto;
                cuota.MontoCobro = 0;
                //ver fecha cobro
            }

            // Modificar saldo para la 1er cuota
            cobroModificado.Cuotas.First().Saldo = saldoCuota;

            foreach (var cobro in cobrosVenta.Where(c => c.Id >= cobroModificado.Id))
            {
                var montoCobro = cobro.Monto;
                cobro.Cuotas.Clear();

                while (montoCobro > 0)
                {
                    var cuota = cuotas.FirstOrDefault(c => c.Saldo > 0);
                    if (cuota == null)
                    {
                        // El monto cobrado supera al saldo de la venta
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
                        // Cuota con saldo deudor, queda PARCIAL (si esta vencida va a quedar ATRASADA)
                        cuota.MontoCobro += montoCobro;
                        cuota.Estado = EstadoCuota.Parcial;
                        montoCobro = 0;
                    }

                    cuota.Saldo = cuota.Monto - cuota.MontoCobro;
                    cuota.FechaCobro = cobro.FechaCobro;
                    cobro.Cuotas.Add(cuota);
                }
            }

            VentaService.ActualizarEstadoCuotas(venta);
            VentaService.ActualizarEstadoVenta(venta);
        }

        #region Private Methods

        private void CalcularMontos(RendicionDominio rendicion)
        {
            rendicion.MontoFacturado = rendicion.Cobros.Sum(c => c.Monto);
            rendicion.MontoNeto = rendicion.MontoFacturado - rendicion.MontoComision;
        }

        #endregion
    }
}
