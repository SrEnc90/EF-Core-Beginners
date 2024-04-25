using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sol_EFCorePeliculas.DTOs;
using Sol_EFCorePeliculas.Entidades;

namespace Sol_EFCorePeliculas.Controllers;

[ApiController]
[Route("api/actores")]
public class ActoresController: ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public ActoresController(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Actor>>> Get()
    {
        //Mapeando a un objeto de tipo anónimo 
        // var actores = await _context.Actores
        //     .Select(a => new { Id = a.Id, Nombre = a.Nombre }).ToListAsync();//Al no tener una clase específica mapeanos a un tipo anónimo

        //Mapeando a un DTO
        // var actores = await _context.Actores
        //     .Select(a => new ActorDTO() { Id = a.Id, Nombre = a.Nombre }).ToListAsync();
        
        //Utilizando AutoMapper
        var actores = await _context.Actores
            .ProjectTo<ActorDTO>(_mapper.ConfigurationProvider).ToListAsync();
        
        return Ok(actores);
    }
}