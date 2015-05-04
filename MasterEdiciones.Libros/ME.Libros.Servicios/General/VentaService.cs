﻿using System.Linq;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

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
                    if (!ProductoService.VerificarStock(ventaItemDominio.Producto, ventaItemDominio.Cantidad))
                    {
                        foreach (var error in ProductoService.ModelError)
                        {
                            ModelError.Add(error.Key, error.Value);
                        }

                        return -1;
                    }

                    ProductoService.RestarStock(ventaItemDominio.Producto, ventaItemDominio.Cantidad);
                    ventaItemDominio.PrecioCosto = ventaItemDominio.Producto.PrecioCosto;
                    ventaItemDominio.PrecioVenta = ventaItemDominio.Producto.PrecioVenta;
                    CalcularTotalItem(ventaItemDominio);
                }
                CalcularTotalVenta(ventaDominio);
            }
            else
            {
                foreach (var ventaItemDominio in ventaDominio.VentaItems)
                {
                    ProductoService.SumarStock(ventaItemDominio.Producto, ventaItemDominio.Cantidad);
                }
            }

            return base.Guardar(ventaDominio);
        }

        public override bool Validar(VentaDominio entidad)
        {
            //if (entidad.VentaItems.All(ventaItemDominio => ProductoService.VerificarStock(ventaItemDominio.Producto, ventaItemDominio.Cantidad)))
            //{ 
            //    return base.Validar(entidad);
            //}

            //foreach (var error in ProductoService.ModelError)
            //{
            //    ModelError.Add(error.Key, error.Value);
            //}

            if (entidad.VentaItems.Count == 0)
            {
                ModelError.Add("Error", ErrorMessages.RequiredItems);
            }

            return base.Validar(entidad);
        }

        private static void CalcularTotalItem(VentaItemDominio ventaItemDominio)
        {
            ventaItemDominio.Monto = ventaItemDominio.Cantidad * ventaItemDominio.Producto.PrecioVenta;
        }

        private static void CalcularTotalVenta(VentaDominio entidad)
        {
            entidad.MontoCalculado = entidad.VentaItems.Sum(vi => vi.Monto);
        }
    }
}
