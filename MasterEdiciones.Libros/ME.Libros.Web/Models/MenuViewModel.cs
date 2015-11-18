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
            Hijos = new List<MenuViewModel>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Link { get; set; }
        public string Controller { get; set; }
        public int Posicion { get; set; }
        public bool Seleccionado { get; set; }
        public bool TieneHijos
        {
            get { return Hijos.Any(); }
        }

        public List<MenuViewModel> Hijos { get; set; }
    }

    public class NavigationBarViewModel
    {
        public List<MenuViewModel> MenuViewModels { get; set; }
        public MenuViewModel MenuSeleccionado { get; set; }
        public bool MostrarMenu { get; set; }
        public bool MostrarSubMenu { get; set; }
        public string Title { get; set; }

        public NavigationBarViewModel()
        {
            MenuViewModels = new List<MenuViewModel>();
            LoadMenues();
            MenuSeleccionado = MenuViewModels.FirstOrDefault(m => m.Seleccionado);
            if (MenuSeleccionado != null)
            {
                MostrarMenu = true;
                if (MenuSeleccionado.Hijos.Any())
                {
                    MostrarSubMenu = true;
                    var subMenu = MenuSeleccionado.Hijos.FirstOrDefault(m => m.Seleccionado);
                    if (subMenu != null)
                    {
                        Title = subMenu.Nombre;
                    }
                }
                else
                {
                    Title = MenuSeleccionado.Nombre;
                }
            }
        }

        private void LoadMenues()
        {
            var helper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            var currentController = HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();

            // Crear menues y submenues
            var menuAdministracion = new MenuViewModel
            {
                Nombre = "Administración",
                Link = helper.Action("Index", "Cliente"),
                Controller = "Cliente",
                Posicion = 0,
                Hijos = new List<MenuViewModel>
                {
                    new MenuViewModel
                    {
                        Id = 1,
                        Nombre = "Clientes",
                        Controller = "Cliente",
                        Link = helper.Action("Index", "Cliente"),
                        Posicion = 10
                    },
                    new MenuViewModel
                    {
                        Id = 2,
                        Nombre = "Cobradores",
                        Controller = "Cobrador",
                        Link = helper.Action("Index", "Cobrador"),
                        Posicion = 20
                    },
                    new MenuViewModel
                    {
                        Id = 3,
                        Nombre = "Localidades",
                        Controller = "Localidad",
                        Link = helper.Action("Index", "Localidad"),
                        Posicion = 50
                    },
                    new MenuViewModel
                    {
                        Id = 4,
                        Nombre = "Rubros",
                        Controller = "Rubro",
                        Link = helper.Action("Index", "Rubro"),
                        Posicion = 80
                    },
                    new MenuViewModel
                    {
                        Id = 5,
                        Nombre = "Editoriales",
                        Controller = "Editorial",
                        Link = helper.Action("Index", "Editorial"),
                        Posicion = 90
                    },
                    new MenuViewModel
                    {
                        Id = 6,
                        Nombre = "Productos",
                        Controller = "Producto",
                        Link = helper.Action("Index", "Producto"),
                        Posicion = 70
                    },
                    new MenuViewModel
                    {
                        Id = 7,
                        Nombre = "Zonas",
                        Controller = "Zona",
                        Link = helper.Action("Index", "Zona"),
                        Posicion = 50
                    },
                    new MenuViewModel
                    {
                        Id = 8,
                        Nombre = "Gastos",
                        Controller = "Gasto",
                        Link = helper.Action("Index", "Gasto"),
                        Posicion = 100
                    },
                    new MenuViewModel
                    {
                        Id = 9,
                        Nombre = "Vendedores",
                        Controller = "Vendedor",
                        Link = helper.Action("Index", "Vendedor"),
                        Posicion = 30
                    },
                    new MenuViewModel
                    {
                        Id = 10,
                        Nombre = "Proveedores",
                        Controller = "Proveedor",
                        Link = helper.Action("Index", "Proveedor"),
                        Posicion = 40
                    },
                    new MenuViewModel
                    {
                        Id = 12,
                        Nombre = "IVA",
                        Controller = "Iva",
                        Link = helper.Action("Index", "Iva"),
                        Posicion = 110
                    },
                    new MenuViewModel
                    {
                        Id = 24,
                        Nombre = "Planes de Pago",
                        Controller = "PlanPago",
                        Link = helper.Action("Index", "PlanPago"),
                        Posicion = 60,
                    }
                }
            };

            var menuVentas = new MenuViewModel
            {
                Nombre = "Ventas",
                Link = helper.Action("Index", "Venta", new { estado = EstadoVenta.Vigente }),
                Controller = "Venta",
                Posicion = 50,
                Hijos = new List<MenuViewModel>
                {
                    new MenuViewModel
                    {
                        Id = 20,
                        Nombre = "Vigentes",
                        Controller = "Venta",
                        Link = helper.Action("Index", "Venta", new {estado = EstadoVenta.Vigente}),
                        Posicion = 20
                    },
                    new MenuViewModel
                    {
                        Id = 21,
                        Nombre = "Pagadas",
                        Controller = "Venta",
                        Link = helper.Action("Index", "Venta", new { estado = EstadoVenta.Pagada }),
                        Posicion = 30
                    },
                    new MenuViewModel
                    {
                        Id = 22,
                        Nombre = "Anuladas",
                        Controller = "Venta",
                        Link = helper.Action("Index", "Venta", new { estado = EstadoVenta.Anulada }),
                        Posicion = 40
                    },
                    new MenuViewModel
                    {
                        Id = 23,
                        Nombre = "Todas",
                        Controller = "Venta",
                        Link = helper.Action("Index", "Venta", new { estado = (EstadoVenta?)null }),
                        Posicion = 50,
                    }
                }
            };

            var menuCompras = new MenuViewModel
            {
                Nombre = "Compras",
                Link = helper.Action("Index", "Compra", new { estado = EstadoCompra.Pagada }),
                Controller = "Compra",
                Posicion = 100,
                Hijos = new List<MenuViewModel>
                {
                    new MenuViewModel
                    {
                        Id = 26,
                        Nombre = "Todas",
                        Controller = "Compra",
                        Link = helper.Action("Index", "Compra", new { estado = (EstadoCompra?)null }),
                        Posicion = 90,
                    },
                    new MenuViewModel
                    {
                        Id = 27,
                        Nombre = "Pagadas",
                        Controller = "Compra",
                        Link = helper.Action("Index", "Compra", new { estado = EstadoCompra.Pagada }),
                        Posicion = 70
                    },
                    new MenuViewModel
                    {
                        Id = 28,
                        Nombre = "Anuladas",
                        Controller = "Compra",
                        Link = helper.Action("Index", "Compra", new { estado = EstadoCompra.Anulada }),
                        Posicion = 80
                    }
                }
            };

            var menuTesoreria = new MenuViewModel
            {
                Nombre = "Tesoseria",
                Link = helper.Action("Index", "Rendicion"),
                Controller = "Rendicion",
                Posicion = 150,
                Hijos = new List<MenuViewModel>
                {
                    new MenuViewModel
                    {
                        Id = 100,
                        Nombre = "Rendiciones",
                        Controller = "Rendicion",
                        Link = helper.Action("Index", "Rendicion"),
                        Posicion = 10
                    }
                }
            };

            var menuReporte = new MenuViewModel
            {
                Nombre = "Reportes",
                Link = helper.Action("Index", "Reporte"),
                Controller = "Reporte",
                Posicion = 200,
                Hijos = new List<MenuViewModel>
                {
                    new MenuViewModel
                    {
                        Id = 150,
                        Nombre = "Dashboard",
                        Controller = "Reporte",
                        Link = helper.Action("Index", "Reporte"),
                        Posicion = 10
                    }
                }
            };

            var menuSistema = new MenuViewModel
            {
                Nombre = "Sistema",
                Link = helper.Action("Index", "Usuario"),
                Controller = "Usuario",
                Posicion = 250,
                Hijos = new List<MenuViewModel>
                {
                    new MenuViewModel
                    {
                        Id = 11,
                        Nombre = "Usuarios",
                        Controller = "Usuario",
                        Link = helper.Action("Index", "Usuario"),
                        Posicion = 10
                    }
                }
            };

            // Seleccionar los menues en base al controlador que se haya ejecutado
            var subMenuAdministracion = menuAdministracion.Hijos.FirstOrDefault(x => x.Controller.Equals(currentController));
            var subMenuTesoreria = menuTesoreria.Hijos.FirstOrDefault(x => x.Controller.Equals(currentController));
            var subMenuReporte = menuReporte.Hijos.FirstOrDefault(x => x.Controller.Equals(currentController));
            var subMenuSistema = menuSistema.Hijos.FirstOrDefault(x => x.Controller.Equals(currentController));

            if (subMenuAdministracion != null)
            {
                subMenuAdministracion.Seleccionado = true;
                menuAdministracion.Seleccionado = true;
            }
            else if (currentController.Equals("Venta"))
            {
                menuVentas.Seleccionado = true;
            }
            else if (currentController.Equals("Compra"))
            {
                menuCompras.Seleccionado = true;
            }
            else if (subMenuTesoreria != null)
            {
                subMenuTesoreria.Seleccionado = true;
                menuTesoreria.Seleccionado = true;
            }
            else if (subMenuReporte != null)
            {
                subMenuReporte.Seleccionado = true;
                menuReporte.Seleccionado = true;
            }
            else if (subMenuSistema != null)
            {
                subMenuSistema.Seleccionado = true;
                menuSistema.Seleccionado = true;
            }

            MenuViewModels.Add(menuAdministracion);
            MenuViewModels.Add(menuVentas);
            MenuViewModels.Add(menuCompras);
            MenuViewModels.Add(menuTesoreria);
            MenuViewModels.Add(menuReporte);
            MenuViewModels.Add(menuSistema);
        }
    }
}