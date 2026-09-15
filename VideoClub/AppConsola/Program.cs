using AccesoDatos.Data;
using AccesoDatos.Models;
using AccesoDatos.Repositories;
using Microsoft.EntityFrameworkCore;

IGenericRepository<Pelicula> peliculaRepository =
    new GenericRepository<Pelicula>();

IGenericRepository<Socio> socioRepository =
    new GenericRepository<Socio>();

IGenericRepository<Alquiler> alquilerRepository =
    new GenericRepository<Alquiler>();

bool continuar = true;

while (continuar)
{
    Console.Clear();

    Console.WriteLine("==============================");
    Console.WriteLine("          VIDEOCLUB");
    Console.WriteLine("==============================");
    Console.WriteLine("1. Registrar película");
    Console.WriteLine("2. Registrar socio");
    Console.WriteLine("3. Registrar alquiler");
    Console.WriteLine("4. Devolver alquiler");
    Console.WriteLine("5. Mostrar socios");
    Console.WriteLine("6. Eliminar socio");
    Console.WriteLine("7. Reporte de alquileres por socio");
    Console.WriteLine("8. Socios con demora");
    Console.WriteLine("9. Películas más alquiladas");
    Console.WriteLine("10. Socio que más películas alquiló");
    Console.WriteLine("0. Salir");
    Console.WriteLine("==============================");

    Console.Write("Seleccione una opción: ");
    string opcion = Console.ReadLine();

    switch (opcion)
    {
        case "1":
            RegistrarPelicula();
            break;

        case "2":
            RegistrarSocio();
            break;

        case "3":
            RegistrarAlquiler();
            break;

        case "4":
            DevolverAlquiler();
            break;

        case "5":
            MostrarSocios();
            break;

        case "6":
            EliminarSocio();
            break;

        case "7":
            ReporteAlquileresPorSocio();
            break;

        case "8":
            ReporteSociosConDemora();
            break;

        case "9":
            ReportePeliculasMasAlquiladas();
            break;

        case "10":
            ReporteSocioQueMasAlquilo();
            break;

        case "0":
            continuar = false;
            break;

        default:
            Console.WriteLine("Opción inválida.");
            Console.ReadKey();
            break;
    }
}


// =====================================================
// 1. REGISTRAR PELÍCULA
// =====================================================

void RegistrarPelicula()
{
    Console.Clear();

    Console.WriteLine("===== REGISTRAR PELÍCULA =====");

    Console.Write("Título: ");
    string titulo = Console.ReadLine();

    Console.Write("Autor: ");
    string autor = Console.ReadLine();

    Console.Write("Cantidad disponible: ");
    int cantidad = int.Parse(Console.ReadLine());

    Pelicula pelicula = new Pelicula
    {
        Titulo = titulo,
        Autor = autor,
        CantidadDisponible = cantidad
    };

    peliculaRepository.Agregar(pelicula);

    Console.WriteLine();
    Console.WriteLine("Película registrada correctamente.");

    Console.ReadKey();
}


// =====================================================
// 2. REGISTRAR SOCIO
// =====================================================

void RegistrarSocio()
{
    Console.Clear();

    Console.WriteLine("===== REGISTRAR SOCIO =====");

    Console.Write("Nombre: ");
    string nombre = Console.ReadLine();

    Console.Write("Apellido: ");
    string apellido = Console.ReadLine();

    Console.Write("DNI: ");
    string dni = Console.ReadLine();

    Console.Write("Teléfono: ");
    string telefono = Console.ReadLine();

    Socio socio = new Socio
    {
        Nombre = nombre,
        Apellido = apellido,
        DNI = dni,
        Telefono = telefono
    };

    socioRepository.Agregar(socio);

    Console.WriteLine();
    Console.WriteLine("Socio registrado correctamente.");
    Console.WriteLine($"El ID asignado es: {socio.Id}");

    Console.ReadKey();
}


// =====================================================
// 3. REGISTRAR ALQUILER
// =====================================================

void RegistrarAlquiler()
{
    Console.Clear();

    Console.WriteLine("===== REGISTRAR ALQUILER =====");

    // ---------------------------------------------
    // MOSTRAR SOCIOS
    // ---------------------------------------------

    List<Socio> socios = socioRepository.ObtenerTodos();

    if (socios.Count == 0)
    {
        Console.WriteLine("No hay socios registrados.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine();
    Console.WriteLine("SOCIOS:");

    foreach (Socio socio in socios)
    {
        Console.WriteLine(
            $"ID: {socio.Id} - {socio.Nombre} {socio.Apellido} - DNI: {socio.DNI}");
    }

    Console.WriteLine();

    Console.Write("Ingrese el ID del socio: ");
    int socioId = int.Parse(Console.ReadLine());

    Socio socioSeleccionado = socioRepository.ObtenerPorId(socioId);

    if (socioSeleccionado == null)
    {
        Console.WriteLine("El socio no existe.");
        Console.ReadKey();
        return;
    }


    // ---------------------------------------------
    // MOSTRAR PELÍCULAS
    // ---------------------------------------------

    List<Pelicula> peliculas = peliculaRepository.ObtenerTodos();

    if (peliculas.Count == 0)
    {
        Console.WriteLine("No hay películas registradas.");
        Console.ReadKey();
        return;
    }

    Console.WriteLine();
    Console.WriteLine("PELÍCULAS DISPONIBLES:");

    foreach (Pelicula pelicula in peliculas)
    {
        Console.WriteLine(
            $"ID: {pelicula.Id} - {pelicula.Titulo} - Disponibles: {pelicula.CantidadDisponible}");
    }


    // ---------------------------------------------
    // CREAR ALQUILER
    // ---------------------------------------------

    Alquiler alquiler = new Alquiler
    {
        SocioId = socioId,
        FechaAlquiler = DateTime.Now,
        Devuelto = false
    };


    // ---------------------------------------------
    // CANTIDAD DE PELÍCULAS
    // ---------------------------------------------

    Console.WriteLine();

    Console.Write("¿Cuántas películas desea alquilar?: ");
    int cantidadPeliculas = int.Parse(Console.ReadLine());

    decimal precioPorDia = 100;

    decimal montoTotal = 0;


    // ---------------------------------------------
    // AGREGAR PELÍCULAS AL ALQUILER
    // ---------------------------------------------

    for (int i = 0; i < cantidadPeliculas; i++)
    {
        Console.WriteLine();

        Console.Write($"Ingrese el ID de la película {i + 1}: ");
        int peliculaId = int.Parse(Console.ReadLine());

        Pelicula peliculaSeleccionada =
            peliculaRepository.ObtenerPorId(peliculaId);

        if (peliculaSeleccionada == null)
        {
            Console.WriteLine("La película no existe.");
            i--;
            continue;
        }

        if (peliculaSeleccionada.CantidadDisponible <= 0)
        {
            Console.WriteLine("No hay unidades disponibles.");
            i--;
            continue;
        }


        // Disminuimos la cantidad disponible
        peliculaSeleccionada.CantidadDisponible--;

        peliculaRepository.Modificar(peliculaSeleccionada);


        // Agregamos la película al alquiler
        AlquilerPelicula alquilerPelicula = new AlquilerPelicula
        {
            PeliculaId = peliculaId,
            Cantidad = 1
        };

        alquiler.Peliculas.Add(alquilerPelicula);


        // Sumamos el precio
        montoTotal += precioPorDia;
    }


    // ---------------------------------------------
    // FECHA DE DEVOLUCIÓN
    // ---------------------------------------------

    Console.WriteLine();

    Console.Write("¿Cuántos días desea alquilar las películas?: ");
    int diasAlquiler = int.Parse(Console.ReadLine());

    alquiler.FechaDevolucion =
        alquiler.FechaAlquiler.AddDays(diasAlquiler);

    alquiler.Monto = montoTotal * diasAlquiler;


    // ---------------------------------------------
    // GUARDAR ALQUILER
    // ---------------------------------------------

    alquilerRepository.Agregar(alquiler);

    Console.WriteLine();
    Console.WriteLine("Alquiler registrado correctamente.");

    Console.WriteLine($"Monto total: ${alquiler.Monto}");

    Console.WriteLine(
        $"Fecha de devolución: {alquiler.FechaDevolucion}");

    Console.ReadKey();
}


// =====================================================
// 4. DEVOLVER ALQUILER
// =====================================================

void DevolverAlquiler()
{
    Console.Clear();

    Console.WriteLine("===== DEVOLVER ALQUILER =====");

    using (AplicationDbContext context = new AplicationDbContext())
    {
        List<Alquiler> alquileres = context.Alquileres
            .Include(a => a.Socio)
            .Include(a => a.Peliculas)
            .ThenInclude(ap => ap.Pelicula)
            .Where(a => !a.Devuelto)
            .ToList();


        if (alquileres.Count == 0)
        {
            Console.WriteLine("No hay alquileres pendientes.");
            Console.ReadKey();
            return;
        }


        foreach (Alquiler alquiler in alquileres)
        {
            Console.WriteLine();
            Console.WriteLine($"Alquiler ID: {alquiler.Id}");

            Console.WriteLine(
                $"Socio: {alquiler.Socio.Nombre} {alquiler.Socio.Apellido}");

            Console.WriteLine(
                $"Fecha de devolución: {alquiler.FechaDevolucion}");

            Console.WriteLine(
                $"Monto original: ${alquiler.Monto}");
        }


        Console.WriteLine();

        Console.Write("Ingrese el ID del alquiler a devolver: ");
        int alquilerId = int.Parse(Console.ReadLine());


        Alquiler alquilerSeleccionado = context.Alquileres
            .Include(a => a.Peliculas)
            .ThenInclude(ap => ap.Pelicula)
            .FirstOrDefault(a => a.Id == alquilerId);


        if (alquilerSeleccionado == null)
        {
            Console.WriteLine("El alquiler no existe.");
            Console.ReadKey();
            return;
        }


        if (alquilerSeleccionado.Devuelto)
        {
            Console.WriteLine("Este alquiler ya fue devuelto.");
            Console.ReadKey();
            return;
        }


        // ---------------------------------------------
        // DEVOLVER LAS PELÍCULAS
        // ---------------------------------------------

        foreach (AlquilerPelicula alquilerPelicula
            in alquilerSeleccionado.Peliculas)
        {
            alquilerPelicula.Pelicula.CantidadDisponible +=
                alquilerPelicula.Cantidad;
        }


        // ---------------------------------------------
        // CALCULAR DEMORA
        // ---------------------------------------------

        DateTime fechaActual = DateTime.Now;

        if (fechaActual > alquilerSeleccionado.FechaDevolucion)
        {
            TimeSpan diferencia =
                fechaActual - alquilerSeleccionado.FechaDevolucion;

            int diasDemora = diferencia.Days;

            decimal recargo =
                alquilerSeleccionado.Monto *
                0.10m *
                diasDemora;

            alquilerSeleccionado.Monto += recargo;

            Console.WriteLine();
            Console.WriteLine(
                $"El alquiler tuvo {diasDemora} día(s) de demora.");

            Console.WriteLine(
                $"Recargo: ${recargo}");
        }


        alquilerSeleccionado.Devuelto = true;

        context.SaveChanges();


        Console.WriteLine();
        Console.WriteLine("Alquiler devuelto correctamente.");

        Console.WriteLine(
            $"Monto final: ${alquilerSeleccionado.Monto}");
    }

    Console.ReadKey();
}


// =====================================================
// 5. MOSTRAR SOCIOS
// =====================================================

void MostrarSocios()
{
    Console.Clear();

    Console.WriteLine("===== SOCIOS REGISTRADOS =====");

    List<Socio> socios = socioRepository.ObtenerTodos();

    if (socios.Count == 0)
    {
        Console.WriteLine();
        Console.WriteLine("No hay socios registrados.");
    }
    else
    {
        foreach (Socio socio in socios)
        {
            Console.WriteLine();
            Console.WriteLine($"ID: {socio.Id}");
            Console.WriteLine($"Nombre: {socio.Nombre}");
            Console.WriteLine($"Apellido: {socio.Apellido}");
            Console.WriteLine($"DNI: {socio.DNI}");
            Console.WriteLine($"Teléfono: {socio.Telefono}");
            Console.WriteLine("------------------------------");
        }
    }

    Console.ReadKey();
}


// =====================================================
// 6. ELIMINAR SOCIO
// =====================================================

void EliminarSocio()
{
    Console.Clear();

    Console.WriteLine("===== ELIMINAR SOCIO =====");

    List<Socio> socios = socioRepository.ObtenerTodos();

    if (socios.Count == 0)
    {
        Console.WriteLine();
        Console.WriteLine("No hay socios registrados.");
        Console.ReadKey();
        return;
    }


    // Mostrar socios antes de pedir el ID

    Console.WriteLine();

    foreach (Socio socio in socios)
    {
        Console.WriteLine(
            $"ID: {socio.Id} - {socio.Nombre} {socio.Apellido} - DNI: {socio.DNI}");
    }


    Console.WriteLine();

    Console.Write("Ingrese el ID del socio que desea eliminar: ");
    int id = int.Parse(Console.ReadLine());


    Socio socioEliminar =
        socioRepository.ObtenerPorId(id);


    if (socioEliminar == null)
    {
        Console.WriteLine();
        Console.WriteLine("No existe un socio con ese ID.");
    }
    else
    {
        socioRepository.Eliminar(id);

        Console.WriteLine();
        Console.WriteLine("Socio eliminado correctamente.");
    }

    Console.ReadKey();
}


// =====================================================
// 7. REPORTE DE ALQUILERES POR SOCIO
// =====================================================

void ReporteAlquileresPorSocio()
{
    Console.Clear();

    Console.WriteLine("===== ALQUILERES POR SOCIO =====");

    using (AplicationDbContext context = new AplicationDbContext())
    {
        List<Socio> socios = context.Socios
            .Include(s => s.Alquileres)
            .ToList();


        foreach (Socio socio in socios)
        {
            Console.WriteLine();

            Console.WriteLine(
                $"Socio: {socio.Nombre} {socio.Apellido}");

            Console.WriteLine(
                $"DNI: {socio.DNI}");

            Console.WriteLine(
                $"Cantidad de alquileres: {socio.Alquileres.Count}");

            Console.WriteLine("------------------------------");
        }
    }

    Console.ReadKey();
}


// =====================================================
// 8. SOCIOS CON DEMORA
// =====================================================

void ReporteSociosConDemora()
{
    Console.Clear();

    Console.WriteLine("===== SOCIOS CON DEMORA =====");

    using (AplicationDbContext context = new AplicationDbContext())
    {
        DateTime fechaActual = DateTime.Now;

        List<Alquiler> alquileresConDemora =
            context.Alquileres
                .Include(a => a.Socio)
                .Where(a =>
                    !a.Devuelto &&
                    a.FechaDevolucion < fechaActual)
                .ToList();


        if (alquileresConDemora.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("No hay socios con demora.");
        }
        else
        {
            foreach (Alquiler alquiler
                in alquileresConDemora)
            {
                int diasDemora =
                    (fechaActual - alquiler.FechaDevolucion).Days;

                Console.WriteLine();

                Console.WriteLine(
                    $"Socio: {alquiler.Socio.Nombre} {alquiler.Socio.Apellido}");

                Console.WriteLine(
                    $"DNI: {alquiler.Socio.DNI}");

                Console.WriteLine(
                    $"Días de demora: {diasDemora}");

                Console.WriteLine("------------------------------");
            }
        }
    }

    Console.ReadKey();
}


// =====================================================
// 9. PELÍCULAS MÁS ALQUILADAS
// =====================================================

void ReportePeliculasMasAlquiladas()
{
    Console.Clear();

    Console.WriteLine("===== PELÍCULAS MÁS ALQUILADAS =====");

    using (AplicationDbContext context = new AplicationDbContext())
    {
        var peliculas = context.AlquileresPeliculas
            .Include(ap => ap.Pelicula)
            .GroupBy(ap => new
            {
                ap.PeliculaId,
                ap.Pelicula.Titulo
            })
            .Select(grupo => new
            {
                Titulo = grupo.Key.Titulo,
                Cantidad = grupo.Sum(ap => ap.Cantidad)
            })
            .OrderByDescending(x => x.Cantidad)
            .ToList();


        if (peliculas.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("Todavía no hay alquileres.");
        }
        else
        {
            foreach (var pelicula in peliculas)
            {
                Console.WriteLine();

                Console.WriteLine(
                    $"Película: {pelicula.Titulo}");

                Console.WriteLine(
                    $"Cantidad de veces alquilada: {pelicula.Cantidad}");

                Console.WriteLine("------------------------------");
            }
        }
    }

    Console.ReadKey();
}


// =====================================================
// 10. SOCIO QUE MÁS PELÍCULAS ALQUILÓ
// =====================================================

void ReporteSocioQueMasAlquilo()
{
    Console.Clear();

    Console.WriteLine("===== SOCIO QUE MÁS PELÍCULAS ALQUILÓ =====");

    using (AplicationDbContext context = new AplicationDbContext())
    {
        var alquileres = context.Alquileres
            .Include(a => a.Socio)
            .Include(a => a.Peliculas)
            .ToList();


        if (alquileres.Count == 0)
        {
            Console.WriteLine();
            Console.WriteLine("Todavía no hay alquileres.");
        }
        else
        {
            var resultado = alquileres
                .GroupBy(a => new
                {
                    a.SocioId,
                    a.Socio.Nombre,
                    a.Socio.Apellido
                })
                .Select(grupo => new
                {
                    Nombre = grupo.Key.Nombre,
                    Apellido = grupo.Key.Apellido,
                    Cantidad = grupo
                        .SelectMany(a => a.Peliculas)
                        .Sum(ap => ap.Cantidad)
                })
                .OrderByDescending(x => x.Cantidad)
                .First();


            Console.WriteLine();

            Console.WriteLine(
                $"Socio: {resultado.Nombre} {resultado.Apellido}");

            Console.WriteLine(
                $"Cantidad de películas alquiladas: {resultado.Cantidad}");
        }
    }

    Console.ReadKey();
}