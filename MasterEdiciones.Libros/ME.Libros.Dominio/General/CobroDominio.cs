using System;
using System.Collections.Generic;
using ME.Libros.Utils.Enums;

namespace ME.Libros.Dominio.General
{
    public class CobroDominio : BaseDominio
    {
        public virtual DateTime FechaCobro { get; set; }
        public virtual decimal Monto { get; set; }
        public virtual EstadoCobro Estado { get; set; }
        public virtual List<CuotaDominio> Cuotas { get; set; }
        public virtual RendicionDominio Rendicion { get; set; }
    }
}
