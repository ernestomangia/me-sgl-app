using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Web.Models
{
    public class MenuViewModel
    {
        public MenuViewModel()
        {
            Seleccionado = false;
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Link { get; set; }
        public int Posicion { get; set; }
        public bool Seleccionado { get; set; }
    }

    public class Menues
    {
        public List<MenuViewModel> MenuViewModels { get; set; } 

        public Menues(int menuId)
        {
            MenuViewModels = new List<MenuViewModel>();
            var helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            switch (menuId)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 12:
                case 24:
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 1,
                        Nombre = "Clientes",
                        Link = helper.Action("Index", "Cliente"),
                        Posicion = 10,
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 2,
                        Nombre = "Cobradores",
                        Link = helper.Action("Index", "Cobrador"),
                        Posicion = 20
                    });
         
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 3,
                        Nombre = "Localidades",
                        Link = helper.Action("Index", "Localidad"),
                        Posicion = 50
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 4,
                        Nombre = "Rubros",
                        Link = helper.Action("Index", "Rubro"),
                        Posicion = 80
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 5,
                        Nombre = "Editoriales",
                        Link = helper.Action("Index", "Editorial"),
                        Posicion = 90
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 6,
                        Nombre = "Productos",
                        Link = helper.Action("Index", "Producto"),
                        Posicion = 70
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 7,
                        Nombre = "Zonas",
                        Link = helper.Action("Index", "Zona"),
                        Posicion = 50
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 8,
                        Nombre = "Gastos",
                        Link = helper.Action("Index", "Gasto"),
                        Posicion = 100
                    });
                               MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 9,
                        Nombre = "Vendedores",
                        Link = helper.Action("Index", "Vendedor"),
                        Posicion = 30
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 10,
                        Nombre = "Proveedores",
                        Link = helper.Action("Index", "Proveedor"),
                        Posicion = 40
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 12,
                        Nombre = "Iva",
                        Link = helper.Action("Index", "Iva"),
                        Posicion = 110
                    });


                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 24,
                        Nombre = "Planes de Pago",
                        Link = helper.Action("Index", "PlanPago"),
                        Posicion = 60,
                    });
                    break;
               case 20:
               case 21:
               case 22:
               case 23:
                case 30:
                case 40:
                case 50:
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 20,
                        Nombre = "Vigentes",
                        Link = helper.Action("Index", "Venta", new {estado = EstadoVenta.Vigente}),
                        Posicion = 20
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 21,
                        Nombre = "Pagadas",
                        Link = helper.Action("Index", "Venta", new {estado = EstadoVenta.Pagada}),
                        Posicion = 30
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 22,
                        Nombre = "Anuladas",
                        Link = helper.Action("Index", "Venta", new {estado = EstadoVenta.Anulada}),
                        Posicion = 40
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 23,
                        Nombre = "Todas",
                        Link = helper.Action("Index", "Venta", new {estado = (EstadoVenta?) null}),
                        Posicion = 50,
                    });
                    break;
                case 25:
                     MenuViewModels.Add(new MenuViewModel
                    {
                       Id = 25,
                        Nombre = "Cobros",
                        Link = helper.Action("Index", "Cobro"),
                        Posicion = 50,
                    });
                    break;
                case 26:
                case 27:
                case 28:
                     MenuViewModels.Add(new MenuViewModel
                     {
                        Id = 26,
                         Nombre = "Todas",
                        Link = helper.Action("Index", "Compra"),
                        Posicion = 60,
                     });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 27,
                        Nombre = "Pagadas",
                        Link = helper.Action("Index", "Compra", new {estado = EstadoCompra.Pagada}),
                        Posicion = 70
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 28,
                        Nombre = "Anuladas",
                        Link = helper.Action("Index", "Compra", new { estado = EstadoCompra.Anulada }),
                        Posicion = 80
                    });
                    break;
                case 11:
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 11,
                        Nombre = "Usuarios",
                        Link = helper.Action("Index", "Usuario"),
                        Posicion = 10,
                    });

                    break;
            }
            Seleccionar(menuId);
        }

        public void Seleccionar(int id)
        {
            MenuViewModels.First(m => m.Id == id).Seleccionado = true;
        }
    } 
}