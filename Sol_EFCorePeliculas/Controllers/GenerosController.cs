using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sol_EFCorePeliculas.Entidades;

namespace Sol_EFCorePeliculas.Controllers;

[ApiController]
[Route("api/generos")]
public class GenerosController: ControllerBase
{
    private readonly ApplicationDbContext _context;

    public GenerosController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IEnumerable<Genero>> Get()
    {
        //return await _context.Generos.AsNoTracking().ToListAsync(); //AsNoTracking() le indicamos que va hacer de solo lectura, es decir va a correr más rápido. También podemos configurarlo desde el Program de manera global
        return await _context.Generos
            .OrderBy(g => g.Nombre)
            .ToListAsync(); //AsNoTracking lo estamos colocando de manera global en el Program
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Genero>> Get(int id)
    {
        var genero = await _context.Generos.FirstOrDefaultAsync(g => g.Identificador == id);
        if (genero is null) return NotFound($"No existe el género con el ID: {id}");
        return genero;
    }

    [HttpGet("primer")]
    public async Task<ActionResult<Genero>> Primer()
    {
        //return await _context.Generos.FirstAsync(g => g.Nombre.StartsWith("C"));
        var genero = await _context.Generos.FirstOrDefaultAsync(g => g.Nombre.StartsWith("Z"));
        if (genero is null) return NotFound("No existe el genero buscado");
        return genero;
    }

    [HttpGet("filtrar")]
    public async Task<IEnumerable<Genero>> Filtrar(string nombre)
    {
        return await _context.Generos
            .Where(g => g.Nombre.ToUpper().Contains(nombre.ToUpper()))
            // .OrderByDescending(g=>g.Nombre) //Descendiente
            .OrderBy(g=>g.Nombre) //Ascendiente
            .ToListAsync();
    }

    [HttpGet("paginacion")]
    public async Task<ActionResult<IEnumerable<Genero>>> GetPaginacion(int paginaActual = 1)
    {
        var cantidadRegistrosPorPagina = 2;
        var generos = await _context.Generos
            .Skip((paginaActual - 1) * cantidadRegistrosPorPagina)
            .Take(cantidadRegistrosPorPagina).ToListAsync();
        return generos;
    }

    [HttpPost]
    public async Task<ActionResult> Post(Genero genero)
    {
        //Vamos a ver los estatos en cada línea para ver el seguimiento que le hace entity framework core a genero
        var estatus1 = _context.Entry(genero).State; //estatus1 = Detached(Quiere decir que EF core no le está dando seguimiento al objeto genero)
        _context.Add(genero); //Con indicamos que el estado de genero va hacer el de agregar, no lo estamos insertando, solo estamos cambiando de estado
        var estatus2 = _context.Entry(genero).State; //estatus2 = Added, después de utilozar la función Add sobre el género, el estatus ha cambiado a Added. EF core está marcando el objeto como Added para que después al utilizar saveChanges se guarde
        await _context.SaveChangesAsync();
        var estatus3 = _context.Entry(genero).State; //estatus3 = Unchanged(Sin modificar). Esto quiere decir que ahora EF core sabe que el objeto genero se corresponde con un registro de nuestra base de datos, pero que ahora no ha sido modificado, por lo que si yo hago savechanges() en la línea siguiente no va a cambiar nada porque su estado es unchanged
        return Ok();
    }

    [HttpPost("varios")]
    public async Task<ActionResult> Post(Genero[] generos)
    {
        await _context.AddRangeAsync(generos); //Cambiar de estado todos los generos de un tirón
        
        //Si yo quiero también puedo marcar varios objetos distintos con el estado agregado y al final de todo hacer SaveChangesAsync
        await _context.AddAsync(new Actor());//Ejemplo de como si estuviera agregando un actor
        
        await _context.SaveChangesAsync();
        return Ok();
    }
}