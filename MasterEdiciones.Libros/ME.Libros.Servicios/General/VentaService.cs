using System;
using System.Collections.Generic;
using System.Linq;
using LinqKit;

using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
using ME.Libros.DTO;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Servicios.General
{
    public class VentaService : AbstractService<VentaDominio>
    {
        private ProductoService ProductoService { get; set; }

        #region Constructor(s)

        public VentaService(IRepository<VentaDominio> repository)
            : base(repository)
        {
        }

        public VentaService(IRepository<VentaDominio> repository, ProductoService productoService)
            : base(repository)
        {
            ProductoService = productoService;
        }

        #endregion

        public override long Guardar(VentaDominio ventaDominio)
        {
            if (ventaDominio.Id == 0)
            {
                if (!Validar(ventaDominio))
                {
                    return -1;
                }

                foreach (var ventaItemDominio in ventaDominio.VentaItems)
                {
                    var producto = ventaItemDominio.Producto;
                    ProductoService.RestarStock(producto, ventaItemDominio.Cantidad);
                    ventaItemDominio.PrecioCosto = producto.PrecioCosto;
                    ventaItemDominio.PrecioVentaCalculado = producto.PrecioVenta;
                    CalcularTotalItem(ventaItemDominio);
                }

                // Plan de pago
                if (ventaDominio.PlanPago.Tipo == TipoPlanPago.Financiado)
                {
                    // N cuotas
                    ventaDominio.CantidadCuotas = ventaDominio.PlanPago.CantidadCuotas;
                    ventaDominio.MontoCuota = ventaDominio.PlanPago.Monto;
                    ventaDominio.MontoCobrado = 0;

                    // Plan de pago a partir de montos fijos
                    GenerarCuotas(ventaDominio);
                    CalcularSaldo(ventaDominio);
                    CalcularMontoVendido(ventaDominio);

                    // TODO: Generar plan de pago a partir de un interes
                    ventaDominio.Estado = EstadoVenta.Vigente;
                }
                else
                {
                    // Contado
                    // TODO: Generar cobro porque es contado
                    ventaDominio.Saldo = 0;
                    ventaDominio.MontoCobrado = ventaDominio.MontoVendido;
                    ventaDominio.Estado = EstadoVenta.Pagada;
                }
                
                CalcularMontoNetoVendido(ventaDominio);
                CalcularTotalVenta(ventaDominio);
            }

            return base.Guardar(ventaDominio);
        }

        public override bool Validar(VentaDominio entidad)
        {
            if (entidad.VentaItems.Count == 0)
            {
                ModelError.Add("Error", ErrorMessages.RequiredItems);
            }

            return base.Validar(entidad);
        }

        public long AnularVenta(long id)
        {
            var ventaDominio = GetPorId(id);
            if (ventaDominio.Estado == EstadoVenta.Vigente || ventaDominio.Estado == EstadoVenta.Pagada)
            {
                ventaDominio.Estado = EstadoVenta.Anulada;
                foreach (var ventaItemDominio in ventaDominio.VentaItems)
                {
                    ProductoService.SumarStock(ventaItemDominio.Producto, ventaItemDominio.Cantidad);
                }

                //TODO: las cuotas se anulan? montos/saldo?
            }

            return Guardar(ventaDominio);
        }

        public List<VentaDominio> Listar(VentaSearchDto ventaSearchDto)
        {
            var predicateBuilder = PredicateBuilder.True<VentaDominio>();
            if (ventaSearchDto.EstadoVenta != null)
            {
                predicateBuilder = predicateBuilder.And(v => v.Estado == ventaSearchDto.EstadoVenta);
            }
            if (!string.IsNullOrWhiteSpace(ventaSearchDto.Cliente))
            {
                predicateBuilder = predicateBuilder.And(v => (v.Cliente.Nombre + " " + v.Cliente.Apellido).Contains(ventaSearchDto.Cliente));
            }
            if (!string.IsNullOrWhiteSpace(ventaSearchDto.Cobrador))
            {
                predicateBuilder = predicateBuilder.And(v => (v.Cobrador.Nombre + " " + v.Cobrador.Apellido).Contains(ventaSearchDto.Cobrador));
            }
            if (!string.IsNullOrWhiteSpace(ventaSearchDto.Vendedor))
            {
                predicateBuilder = predicateBuilder.And(v => (v.Vendedor.Nombre + " " + v.Vendedor.Apellido).Contains(ventaSearchDto.Vendedor));
            }
            if (ventaSearchDto.Desde.HasValue)
            {
                predicateBuilder = predicateBuilder.And(v => v.FechaVenta >= ventaSearchDto.Desde);
            }
            if (ventaSearchDto.Hasta.HasValue)
            {
                predicateBuilder = predicateBuilder.And(v => v.FechaVenta <= ventaSearchDto.Hasta);
            }

            return ListarAsQueryable()
                .AsExpandable()
                .Where(predicateBuilder)
                .OrderByDescending(v => v.FechaVenta)
                .ThenByDescending(v => v.Id)
                .ToList();
        }

        public void CalcularSaldo(VentaDominio ventaDominio)
        {
            ventaDominio.Saldo = ventaDominio.Cuotas.Sum(c => c.Saldo);
        }

        public void ContabilizarCobro(VentaDominio ventaDominio, decimal montoCobro)
        {
            ventaDominio.MontoCobrado += montoCobro;
            ventaDominio.Saldo -= montoCobro;
        }

        public void ActualizarEstadoCuotas(VentaDominio ventaDominio)
        {
            // Actualizar estado de cuotas segun la fecha de vencimiento
            foreach (var cuotaDominio in ventaDominio.Cuotas.Where(c => c.Estado != EstadoCuota.Pagada))
            {
                if (cuotaDominio.FechaVencimiento.Date < DateTime.Now.Date)
                {
                    cuotaDominio.Estado = EstadoCuota.Atrasada;
                }
            }
        }

        public void ActualizarEstadoVenta(VentaDominio ventaDominio)
        {
            ventaDominio.Estado = ventaDominio.Saldo > 0
                ? EstadoVenta.Vigente
                : EstadoVenta.Pagada;
        }

        public void CalcularMontoNetoVendido(VentaDominio ventaDominio)
        {
            ventaDominio.MontoNetoVendido = ventaDominio.MontoVendido - ventaDominio.MontoComision;
        }

        #region Private Methods

        private void CalcularTotalItem(VentaItemDominio ventaItemDominio)
        {
            ventaItemDominio.MontoCalculado = ventaItemDominio.Cantidad * ventaItemDominio.PrecioVentaCalculado;
        }

        private void CalcularTotalVenta(VentaDominio entidad)
        {
            entidad.MontoCalculado = entidad.VentaItems.Sum(vi => vi.MontoVendido);
        }

        private void GenerarCuotas(VentaDominio ventaDominio)
        {
            for (var i = 0; i < ventaDominio.CantidadCuotas; i++)
            {
                var fechaVencimiento = ventaDominio.FechaCobro.AddMonths(i).Date;
                var cuota = new CuotaDominio
                {
                    FechaAlta = DateTime.Now,
                    Numero = i + 1,
                    FechaVencimiento = fechaVencimiento,
                    Estado = fechaVencimiento >= DateTime.Now.Date
                        ? EstadoCuota.NoVencida
                        : EstadoCuota.Atrasada,
                    // DiasAtraso = (DateTime.Now.Date - fechaVencimiento).Days,
                    Monto = ventaDominio.MontoCuota,
                    Saldo = ventaDominio.MontoCuota
                };

                ventaDominio.Cuotas.Add(cuota);
            }
        }

        private void CalcularMontoVendido(VentaDominio ventaDominio)
        {
            ventaDominio.MontoVendido = ventaDominio.Cuotas.Sum(c => c.Monto);
        }

        #endregion
    }
}
