using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Sol_EFCorePeliculas.DTOs;
using Sol_EFCorePeliculas.Entidades;

namespace Sol_EFCorePeliculas.Controllers;

[ApiController]
[Route("api/cines")]
public class CinesController: ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CinesController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IEnumerable<CineDTO>> Get()
    {
        return await _context.Cines.ProjectTo<CineDTO>(_mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("cercanos")]
    public async Task<ActionResult> Get(double latitud, double longitud)
    {
        var geometryFactory =
            NtsGeometryServices.Instance
                .CreateGeometryFactory(srid: 4326); //Nos permite hacer mediciones sobre nuestro planeta

        var miUbicacion = geometryFactory.CreatePoint(new Coordinate(longitud, latitud));
        var distanciaMaximaEnMetros = 2000; //2km
        var cines = await _context.Cines
            .OrderBy(c => c.Ubicacion.Distance(miUbicacion))
            .Where(c=>c.Ubicacion.IsWithinDistance(miUbicacion, distanciaMaximaEnMetros)) //IsWithinDistance: significa que va hacer < ó = distanciaMaximaEnMetros
            .Select(c => new
            {
                Nombre = c.Nombre,
                Distancia = Math.Round(c.Ubicacion.Distance(miUbicacion))
            }).ToListAsync();
        
        return Ok(cines);
    }
    
    //Insertar Registros pero con data relacionada
    [HttpPost]
    public async Task<ActionResult> Post()
    {
        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        var ubicacionCine = geometryFactory.CreatePoint(new Coordinate(-69.896979, 18.476276));

        var cine = new Cine()
        {
            Nombre = "Mi cine",
            Ubicacion = ubicacionCine,
            CineOferta = new CineOferta()
            {
                PorcentajeDescuento = 5,
                FechaInicio = DateTime.Today,
                FechaFin = DateTime.Today.AddDays(7)
            },
            SalaDeCines = new HashSet<SalaDeCine>()
            {
                new SalaDeCine()
                {
                    Precio = 200,
                    TipoSalaDeCine = TipoSalaDeCine.DosDimensiones
                },
                new SalaDeCine()
                {
                    Precio = 350,
                    TipoSalaDeCine = TipoSalaDeCine.TresDimensiones
                }
            }
        };

        await _context.AddAsync(cine);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("conDTO")]
    public async Task<ActionResult> Post(CineCreacionDTO cineCreacionDto)
    {
        var cine = _mapper.Map<Cine>(cineCreacionDto);
        await _context.AddAsync(cine);
        await _context.SaveChangesAsync();
        return Ok();
    }
}