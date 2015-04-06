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

        public override void Guardar2(ClienteDominio entidad)
        {
            base.Guardar2(entidad);
        }

        public override bool Validar(ClienteDominio entidad)
        {
            base.Validar(entidad);
            
            // Validad cuil unico
            
            return ModelError.Count == 0;
        }

        #region Private Methods

        #endregion
    }
}
