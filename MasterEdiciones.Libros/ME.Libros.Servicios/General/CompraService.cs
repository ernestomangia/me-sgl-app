using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class CompraService:AbstractService<CompraDominio>
    {

        ProductoService ProductoService { get; set; }

             public CompraService(IRepository<CompraDominio> repository)
            : base(repository)
        {
        }

             public CompraService(IRepository<CompraDominio> repository, ProductoService productoService)
            : base(repository)
        {
            ProductoService = productoService;
        }

    }

}
