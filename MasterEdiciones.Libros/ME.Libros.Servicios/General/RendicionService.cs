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
                    var primerCuotaCobro = cobro.Cuotas.OrderBy(c => c.Numero).First();
                    var venta = primerCuotaCobro.Venta;
                    var cobrosVenta = venta.Cuotas.SelectMany(c => c.Cobros).Distinct().ToList();

                    // Lo cobrado para la 1er cuota del cobro modificado
                    var cobradoPrimerCuotaCobro = cobrosVenta.Where(cob => cob.Id < cobro.Id).Sum(cob => cob.Monto) -
                                     venta.Cuotas.Where(cuota => cuota.Numero < primerCuotaCobro.Numero).Sum(cuota => cuota.Monto);

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
                        cobro.FechaCobro = cobroDto.FechaCobro;
                    }

                    // Recalcular saldo cuotas y relaciones
                    RecalcularCuotas(venta, cobrosVenta, cobro, primerCuotaCobro, cobradoPrimerCuotaCobro);
                }
            }

            CalcularMontos(rendicionDominio);
        }

        public void RecalcularCuotas(VentaDominio venta, List<CobroDominio> cobrosVenta, CobroDominio cobroModificado, CuotaDominio primerCuotaCobro, decimal cobradoPrimerCuotaCobro)
        {
            var cuotas = venta.Cuotas.Where(cuota => cuota.Numero >= primerCuotaCobro.Numero).OrderBy(c => c.Numero).ToList();
            foreach (var cuota in cuotas)
            {
                cuota.Saldo = cuota.Monto;
                cuota.MontoCobro = 0;
                cuota.Estado = EstadoCuota.NoVencida;
                cuota.FechaCobro = null;
            }

            // Modificar saldo para la 1er cuota del cobro modificado
            primerCuotaCobro.Saldo -= cobradoPrimerCuotaCobro;
            primerCuotaCobro.MontoCobro = cobradoPrimerCuotaCobro;

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
