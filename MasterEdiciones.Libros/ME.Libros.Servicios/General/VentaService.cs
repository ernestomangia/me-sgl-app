using System;
using System.Linq;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;
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
                    ProductoService.RestarStock(ventaItemDominio.Producto, ventaItemDominio.Cantidad);
                    ventaItemDominio.PrecioCosto = ventaItemDominio.Producto.PrecioCosto;
                    //ventaItemDominio.PrecioVentaVendido = ventaItemDominio.Producto.PrecioVentaVendido;
                    //CalcularTotalItem(ventaItemDominio);
                }

                // Plan de pago
                if (ventaDominio.PlanPago.Tipo == TipoPlanPago.Financiado)
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
                            // Estado = fechaVencimiento >= DateTime.Now.Date ? EstadoCuota.NoVencida : EstadoCuota.Atrasada,
                            // DiasAtraso = (DateTime.Now.Date - fechaVencimiento).Days,
                            Monto = monto
                        };

                        ventaDominio.Cuotas.Add(cuota);
                    }
                }
                else
                {
                    var monto = ventaDominio.MontoVendido;

                    var cuota = new CuotaDominio
                    {
                        Numero = 1,
                        FechaVencimiento = ventaDominio.FechaCobro,
                        //Estado = EstadoCuota.NoVencida,
                        Monto = monto
                    };

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

        private static void CalcularTotalItem(VentaItemDominio ventaItemDominio)
        {
            ventaItemDominio.MontoVendido = ventaItemDominio.Cantidad * ventaItemDominio.Producto.PrecioVenta;
        }

        private static void CalcularTotalVenta(VentaDominio entidad)
        {
            entidad.MontoCalculado = entidad.VentaItems.Sum(vi => vi.MontoVendido);
        }
    }
}
