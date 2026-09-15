using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using AccesoDatos.Models;

using Microsoft.EntityFrameworkCore;
using AccesoDatos.Models;

namespace AccesoDatos.Data
{
    public class AplicationDbContext : DbContext
    {
        public DbSet<Pelicula> Peliculas { get; set; }

        public DbSet<Socio> Socios { get; set; }

        public DbSet<Alquiler> Alquileres { get; set; }

        public DbSet<AlquilerPelicula> AlquileresPeliculas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                "Data Source=C:\\Users\\catag\\source\\repos\\VideoClub\\VideoClub\\AccesoDatos\\Database\\VideoClub.db");
        }
    }
}
