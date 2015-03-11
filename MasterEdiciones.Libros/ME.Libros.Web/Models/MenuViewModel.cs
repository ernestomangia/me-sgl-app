
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
}