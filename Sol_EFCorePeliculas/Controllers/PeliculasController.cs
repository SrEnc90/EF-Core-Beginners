using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sol_EFCorePeliculas.DTOs;
using Sol_EFCorePeliculas.Entidades;

namespace Sol_EFCorePeliculas.Controllers;

[ApiController]
[Route("api/peliculas")]
public class PeliculasController: ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public PeliculasController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /*
     * Eager Loading: Con esto indicamos explicitamente que queremos cargar la data relacionada a la hora de escribir el query
     * utilizamos para esto .include(En este caso cargamos la data de géneros)
     */
    //Sin ProjectTo(Es de la librería de automapper)
    [HttpGet("{id:int}")] //Parámetro de ruta
    public async Task<ActionResult<PeliculaDTO>> Get(int id)
    {
        var pelicula = await _context.Peliculas
            .Include(p => p.Generos.OrderByDescending(g => g.Nombre))
            //Para traer la data del cine que es una HasSet(lista) ubicado dentro de SalaDeCine, debemos usar el theinclude (me permite traer el tipo de dato ubicado dentro de otro tipo de dato de la consulta original)
            .Include(p => p.SalaDeCines)
            .ThenInclude(s =>
                s.Cine) //Si sale un error es por la variable Point de la clase Cine, con un dto no traemos esa data y lo arreglamos
            .Include(p => p.PeliculasActores.Where(pa => pa.Actor.FechaNacimiento.Value.Year >= 1980))
            .ThenInclude(pa => pa.Actor)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (pelicula is null) return NotFound();

        var peliculaDTO = _mapper.Map<PeliculaDTO>(pelicula); //Trae los cines repetidos

        peliculaDTO.Cines = peliculaDTO.Cines.DistinctBy(c => c.Id).ToList();
        
        return peliculaDTO;
    }
    
    //Con ProjectTo(Ya no necesitamos los .Include)
    /*
     * Esto ocurre xq ProjectTo ve que en nuestro modelo PeliculaDTO hay un collección llamado Generos y entiende que algo se puede realizar con ambos
     * En cuanto a las colecciones Actores y Cines que no se encuentra dentro de Pelicula, ProjectTo revisa el archvo AutoMapperProfile
     * y respeta la configuración que hemos hecho a PeliculaDTO
     */
    [HttpGet("conprojectto/{id:int}")] 
    public async Task<ActionResult<PeliculaDTO>> GetProjectTo(int id)
    {
        var pelicula = await _context.Peliculas
            .ProjectTo<PeliculaDTO>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (pelicula is null) return NotFound();

        pelicula.Cines = pelicula.Cines.DistinctBy(c => c.Id).ToList();
        
        return pelicula;
    }

    [HttpGet("cargadoselectivo{id:int}")]
    public async Task<ActionResult> GetSelectivo(int id)
    {
        //Con el .Select(p => new { }) estamos haciendo un select loading o tb llamado cargado selectivo y estamos mapeando a un objeto anónimo(new { ... })
        var pelicula = await _context.Peliculas.Select(p => new
        {
            Id = p.Id,
            Titulo = p.Titulo,
            Generos = p.Generos.OrderByDescending(g => g.Nombre).Select(g => g.Nombre)
                .ToList(), //Seleccionar y que solo aparezca el nombre y aparte ordernar de manera descendente por nombre
            CantidadDeActores = p.PeliculasActores.Count(), //Número de actores que están en la película
            EnCantidadDeCines = p.SalaDeCines.Select(sc => sc.CineId).Distinct().Count() //Cantidad de cines dónde está la película
        }).FirstOrDefaultAsync(p => p.Id == id);

        if (pelicula is null) return NotFound();

        return Ok(pelicula);
    }

    /*
     * Explicit Loading o carga explícita es cuándo cargamos la entidad y después hacemos otras consultas a la base de datos
     * hacemos más de una consulta(ver la consola) a diferencia del cargado selectivo que hacemos una sola consulta y traemos todo
     */
    [HttpGet("cargadoexplicito{id:int}")]
    public async Task<ActionResult<PeliculaDTO>> GetExplicito(int id)
    {
        //Para poder poder hacer la carga explícita debemos que hacerlo trackeable(recuerda que hicimos no trackeable en el program.cs)
        var pelicula = await _context.Peliculas.AsTracking().FirstOrDefaultAsync(p => p.Id == id);
        /*
         * Ingresamos más código para después hacer otra consulta diferente a la que hicimos antes
         */
        //Ahora hacemos otra consulta diferente a la BBDD para traernos los géneros de las películas
        // await _context.Entry(pelicula).Collection(p => p.Generos).LoadAsync();
        //cantidadGeneros no lo estamos enviando en nuestro json, pero si se puede ver la consulta en la consola
        var cantidadGeneros = await _context.Entry(pelicula)
            .Collection(p => p.Generos).Query().CountAsync();//Contamos la cantidad de géneros
        
        if (pelicula is null) return NotFound();

        var peliculaDTO = _mapper.Map<PeliculaDTO>(pelicula);
        
        return peliculaDTO;
    }
    /*Lazy Loading o carga peresoza es utilizando virtual en los campos de navegación no lo implementé*/

    [HttpGet("agrupadasporestreno")]
    public async Task<ActionResult> GetAgrupadasPorCartelera()
    {
        var peliculasAgrupadas = await _context.Peliculas.GroupBy(p => p.EnCartelera)
                            .Select(g => new
                            {
                                EnCartelera = g.Key, //Se refiere al valor que están dentro del GroupBy(Es decir EnCartelera)
                                Conteo = g.Count(),
                                Peliculas = g.ToList()
                            }).ToListAsync();

        return Ok(peliculasAgrupadas);
    }

    [HttpGet("agrupadasporcantidaddegeneros")]
    public async Task<ActionResult> GetAgrupadasPorCantidadDeGeneros()
    {
        var peliculasAgrupadas = await _context.Peliculas.GroupBy(p => p.Generos.Count())
            .Select(g => new
            {
                ConteoGeneros = g.Key,
                Titulos = g.Select(x => x.Titulo),
                Generos = g.Select(x => x.Generos)
                    .SelectMany(gene => gene)//En vez de tener una colección de Géneros por cada registro, yo voy a tener una única colección dónde están todos los géneros
                    .Select(gene => gene.Nombre) //De los géneros yo voy a traer solo los nombres de cada género
                    .Distinct()
            }).ToListAsync();
        
        return Ok(peliculasAgrupadas);
    }

    //Ejecución diferida
    [HttpGet("filtrar")]
    public async Task<IEnumerable<PeliculaDTO>> Filtrar([FromQuery] PeliculasFiltroDTO peliculasFiltroDto) //El [FromQuery] lo utilizo xq estoy mi endpoint es de tipo Get y está recibiendo como parámetro un tipo de dato complejo
    {
        var peliculasQueryable = _context.Peliculas.AsQueryable(); //El AsQueryable nos permite armar nuestro query paso por paso y ejecutarlo una sola vez al final cuándo lo invocamos de nuevo
        
        if (!string.IsNullOrEmpty(peliculasFiltroDto.Titulo))
        {
            peliculasQueryable =
                peliculasQueryable.Where(p => p.Titulo.ToUpper().Contains(peliculasFiltroDto.Titulo.ToUpper()));
        }

        if (peliculasFiltroDto.EnCartelera)
        {
            peliculasQueryable = peliculasQueryable.Where(p => p.EnCartelera);
        }

        if (peliculasFiltroDto.ProximosEstrenos)
        {
            var hoy = DateTime.Today;
            peliculasQueryable = peliculasQueryable.Where(p => p.FechaEstreno > hoy);
        }

        if (peliculasFiltroDto.GeneroId != 0)
        {
            peliculasQueryable = peliculasQueryable.Where(p =>
                p.Generos.Select(g => g.Identificador).Contains(peliculasFiltroDto.GeneroId));
        }
        
        var peliculas = await peliculasQueryable.Include(p => p.Generos).ToListAsync();
        
        return _mapper.Map<List<PeliculaDTO>>(peliculas);
    }
    
    //Inserción de registros pero con data relacionala existente(Para ello vamos a trabajar con el status)
    [HttpPost]
    public async Task<ActionResult> Post(PeliculaCreacionDTO peliculaCreacionDTO)
    {
        var pelicula = _mapper.Map<Pelicula>(peliculaCreacionDTO);
        //Yo le estoy indicando a EF core que los géneros que estamos pasando acá son géneros de consulta(géneros que ya existen en la bbdd) y que simplemente se quieren agregar como una relación con el objeto película
        pelicula.Generos.ForEach(g => _context.Entry(g).State = EntityState.Unchanged);
        pelicula.SalaDeCines.ForEach(s => _context.Entry(s).State = EntityState.Unchanged);
        //Para crear peliculaActor y no grabar con un campo ya existente
        if (pelicula.PeliculasActores is not null)
        {
            for (int i = 0; i < pelicula.PeliculasActores.Count; i++)
            {
                pelicula.PeliculasActores[i].Orden = i + 1;
            }
        }

        _context.Add(pelicula);
        await _context.SaveChangesAsync();
        return Ok();
    }
}