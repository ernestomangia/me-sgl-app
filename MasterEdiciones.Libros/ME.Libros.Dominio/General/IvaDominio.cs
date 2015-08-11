namespace ME.Libros.Dominio.General
{
    public class IvaDominio : BaseDominio
    {
        #region Properties

        public virtual int Codigo { get; set; }
        public virtual string Nombre { get; set; }
        public virtual decimal Alicuota { get; set; }
        public virtual bool HabilitarEliminar { get; set; }

        #endregion
    }
}
