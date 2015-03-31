using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    public class PlanPagoDominio : BaseDominio
    {
        public virtual string Nombre { get; set; }
        public virtual string Descripcion { get; set; }
        public virtual int CantidadCuotas { get; set; }
        public virtual decimal Monto { get; set; }
        public virtual TipoPlanPago Tipo { get; set; }
    }
}
