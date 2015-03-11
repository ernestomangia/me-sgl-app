using ME.Libros.Api.Repositorios;
using ME.Libros.Dominio.General;

namespace ME.Libros.Servicios.General
{
    public class ClienteService : AbstractService<ClienteDominio>
    {
        public ClienteService(IRepository<ClienteDominio> repository)
            : base(repository)
        {


        }

        public override long Guardar(ClienteDominio entidad)
        {
            if (Validar(entidad))
            {
                return base.Guardar(entidad);
            }

            return -1;
        }

        public override bool Validar(ClienteDominio cliente)
        {
            if (cliente.Cuil.Length != 11)
            {
                ModelError.Add("Cuil", "El campo CUIL debe contener 11 caracteres.");
            }
            if (cliente.TelefonoFijo.Length != 10)
            {
                ModelError.Add("Teléfono", "El campo Teléfono debe contener 10 caracteres.");
            }
            
            return base.Validar(cliente) && ModelError.Count == 0;
        }

        #region Private Methods

        #endregion
    }
}
