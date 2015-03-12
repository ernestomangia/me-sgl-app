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

        public override bool Validar(ClienteDominio entidad)
        {
            base.Validar(entidad);

            if (entidad.Cuil.Length != 11)
            {
                ModelError.Add("Cuil", "El campo CUIL debe contener 11 caracteres.");
            }
            if (!string.IsNullOrEmpty(entidad.TelefonoFijo) && entidad.TelefonoFijo.Length != 10)
            {
                ModelError.Add("Teléfono", "El campo Teléfono debe contener 10 caracteres.");
            }
            
            return ModelError.Count == 0;
        }

        #region Private Methods

        #endregion
    }
}
