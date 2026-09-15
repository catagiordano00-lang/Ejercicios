using System;
using System.Collections.Generic;
using System.Text;

namespace AccesoDatos.Models
{
    public class Pelicula
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Autor { get; set; }

        public int CantidadDisponible { get; set; }

        public List<AlquilerPelicula> Alquileres { get; set; } = new();
    }
}