using AutoMapper;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using Sol_EFCorePeliculas.DTOs;
using Sol_EFCorePeliculas.Entidades;

namespace Sol_EFCorePeliculas.Servicios;

public class AutoMapperProfiles: Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<Actor, ActorDTO>();

        //Mapeando desde la propiedad Ubicación de Cine hacia Longitud y latitud de cineDTO
        CreateMap<Cine, CineDTO>()
            .ForMember(dto => dto.Longitud, ent => ent.MapFrom(prop => prop.Ubicacion.X))
            .ForMember(dto => dto.Latitud, ent => ent.MapFrom(prop => prop.Ubicacion.Y));

        CreateMap<Genero, GeneroDTO>();
        
        //1. Sin ProjectTo
        //Recordemos que la clase Película no tiene una colección de Cines, sino de SalasDeCine , igual que no tiene una colección de Actores
        //Para la propiedad Genero dentro de la clase Película, no tenemos que hacer nada ya que es un mapeo simple(y ya estamos haciendo un mapeo aparte arriba - línea 18)
        CreateMap<Pelicula, PeliculaDTO>()
            .ForMember(dto => dto.Cines,
                ent => ent
                    .MapFrom(prop => prop.SalaDeCines.Select(s => s.Cine)))
            .ForMember(dto => dto.Actores, 
                ent => ent
                    .MapFrom(prop => prop.PeliculasActores.Select(pa => pa.Actor)));
        
        //2. Con ProjectTo(La diferencia es que debemos hacer las configuraciones dentro AutoMapperProfile(cómo order descendemente, etc.))
        // CreateMap<Pelicula, PeliculaDTO>()
        //     .ForMember(dto => dto.Generos, 
        //         ent => ent
        //             .MapFrom(prop => prop.Generos.OrderByDescending(g => g.Nombre)))
        //     .ForMember(dto => dto.Cines,
        //         ent => ent
        //             .MapFrom(prop => prop.SalaDeCines.Select(s => s.Cine)))
        //     .ForMember(dto => dto.Actores,
        //         ent => ent
        //             .MapFrom(prop => prop.PeliculasActores.Select(pa => pa.Actor)));

        var geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        CreateMap<CineCreacionDTO, Cine>()
            .ForMember(ent => ent.Ubicacion,
                dto => dto.MapFrom(campo =>
                    geometryFactory.CreatePoint(new Coordinate(campo.Longitud, campo.Latitud))));

        CreateMap<CineOfertaCreacionDTO, CineOferta>();
        CreateMap<SalaDeCineCreacionDTO, SalaDeCine>();

        CreateMap<PeliculaCreacionDTO, Pelicula>()
            .ForMember(ent => ent.Generos,
                dto =>
                    dto.MapFrom(campo =>
                        campo.GenerosId.Select(id => new Genero()
                        {
                            Identificador = id
                        }))) //Este campo.GenerosId es un listado de entero por lo que tenemos que mapearlo a un listado de objetos de tipo género
            .ForMember(ent => ent.SalaDeCines,
                dto => 
                    dto.MapFrom(campo => 
                        campo.SalasDeCineId.Select(id => new SalaDeCine()
                        {
                            Id = id
                        })));
        CreateMap<PeliculaActorCreacionDTO, PeliculaActor>();
    }
}