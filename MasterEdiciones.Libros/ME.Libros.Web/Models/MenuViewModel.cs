
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                        Link = "#",
                        Posicion = 20
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 9,
                        Nombre = "Vendedores",
                        Link = "#",
                        Posicion = 30
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 10,
                        Nombre = "Proveedores",
                        Link = "#",
                        Posicion = 40
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
                        Posicion = 60
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 5,
                        Nombre = "Editoriales",
                        Link = helper.Action("Index", "Editorial"),
                        Posicion = 70
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 6,
                        Nombre = "Productos",
                        Link = helper.Action("Index", "Producto"),
                        Posicion = 80
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 7,
                        Nombre = "Zonas",
                        Link = "#",
                        Posicion = 90
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 8,
                        Nombre = "Gastos",
                        Link = "#",
                        Posicion = 100
                    });
                    break;
               case 20:
               case 21:
               case 22:
               case 23:
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 20,
                        Nombre = "Reservación",
                        Link = helper.Action("Index", "Reservacion"),
                        Posicion = 10,
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 21,
                        Nombre = "En vigencia",
                        Link = "#",
                        Posicion = 20
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 22,
                        Nombre = "Canceladas",
                        Link = "#",
                        Posicion = 30
                    });
                    MenuViewModels.Add(new MenuViewModel
                    {
                        Id = 23,
                        Nombre = "Finalizadas",
                        Link = "#",
                        Posicion = 40
                    });
                    break;
            }
            Seleccionar(menuId);
        }

        public void Seleccionar (int id)
        {
            MenuViewModels.First(m => m.Id == id).Seleccionado = true;
        }
    } 
}