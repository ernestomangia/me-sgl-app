using System.Linq;
using ME.Libros.Dominio.General;
using ME.Libros.EF;
using ME.Libros.Repositorios;
using ME.Libros.Servicios.General;
using ME.Libros.Web.Models;

namespace ME.Libros.Web.Extensions
{
    public class UsuarioHelper
    {
        private static UsuarioService UsuarioService => new UsuarioService(new EntidadRepository<UsuarioDominio>(new ModelContainer()));

        public static string GetDisplayName(string userName)
        {
            var displayName = string.Empty;
            if (!string.IsNullOrEmpty(userName))
            {
                var usuario = UsuarioService.ListarAsQueryable().SingleOrDefault(u => u.UserName.Equals(userName));
                if (usuario != null)
                {
                    var usuarioViewModel = new UsuarioViewModel(usuario);
                    displayName = string.Format("{0}, {1}", usuarioViewModel.Nombre, usuarioViewModel.Apellido);
                }
            }

            return displayName;
        }

        public static long GetId(string userName)
        {
            long id = 0;
            if (!string.IsNullOrEmpty(userName))
            {
                var usuario = UsuarioService.ListarAsQueryable().SingleOrDefault(u => u.UserName.Equals(userName));
                if (usuario != null)
                {
                    var usuarioViewModel = new UsuarioViewModel(usuario);
                    id = usuarioViewModel.Id;
                }
            }

            return id;
        }
    }
}