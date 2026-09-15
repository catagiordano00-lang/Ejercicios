using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class Alquiler
    {
        public int Id { get; set; }

        public int SocioId { get; set; }
        public Socio Socio { get; set; }

        public DateTime FechaAlquiler { get; set; }

        public DateTime FechaDevolucion { get; set; }

        public decimal Monto { get; set; }

        public bool Devuelto { get; set; }

        public List<AlquilerPelicula> Peliculas { get; set; } = new();
    }
}
