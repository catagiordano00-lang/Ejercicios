using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class AlquilerPelicula
    {
        public int Id { get; set; }

        public int AlquilerId { get; set; }
        public Alquiler Alquiler { get; set; }

        public int PeliculaId { get; set; }
        public Pelicula Pelicula { get; set; }

        public int Cantidad { get; set; }
    }
}
