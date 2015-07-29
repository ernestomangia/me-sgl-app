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
                    // Plan de pago a partir de montos fijos
                    GenerarCuotas(ventaDominio);

                    // TODO: Generar plan de pago a partir de un interes
                    ventaDominio.Estado = EstadoVenta.Vigente;
                }
                else
                {
                    // Contado
                    // Generar cobro porque es contado
                    var cuota = new CuotaDominio
                    {
                        FechaAlta = DateTime.Now,
                        Numero = 0,
                        FechaVencimiento = ventaDominio.FechaVenta,
                        FechaCobro = ventaDominio.FechaVenta,
                        Estado = EstadoCuota.Pagada,
                        Monto = ventaDominio.MontoVendido,
                        MontoCobro = ventaDominio.MontoVendido,
                        DiasAtraso = 0,
                        Interes = 0,
                        Saldo = 0
                    };

                    ventaDominio.Estado = EstadoVenta.Pagada;
                    ventaDominio.Cuotas.Add(cuota);
                }
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
            if (ventaDominio.Estado == EstadoVenta.Vigente)
            {
                ventaDominio.Estado = EstadoVenta.Anulada;
                foreach (var ventaItemDominio in ventaDominio.VentaItems)
                {
                    ProductoService.SumarStock(ventaItemDominio.Producto, ventaItemDominio.Cantidad);
                }
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
            var monto = ventaDominio.PlanPago.Monto;
            for (var i = 0; i < ventaDominio.PlanPago.CantidadCuotas; i++)
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
                    Monto = monto,
                    Saldo = monto
                };

                ventaDominio.Cuotas.Add(cuota);
            }
        }

        #endregion
    }
}
