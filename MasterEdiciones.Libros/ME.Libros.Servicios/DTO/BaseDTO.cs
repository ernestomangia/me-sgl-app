﻿using System;

namespace ME.Libros.Servicios.DTO
{
    public abstract class BaseDTO
    {
        public int Id { get; set; }
        public DateTime FechaAlta { get; set; }
    }
}
