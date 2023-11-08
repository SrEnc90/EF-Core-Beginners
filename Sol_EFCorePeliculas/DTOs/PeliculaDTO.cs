namespace Sol_EFCorePeliculas.DTOs;

public class PeliculaDTO
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    //Cómo en la clase Pelicula hemos cambiado el HashSet por un List. Hay dos formas de cambiar el tipo de dato de Generos
    // public List<GeneroDTO> Generos { get; set; } //1er forma
    public ICollection<GeneroDTO> Generos { get; set; } = new List<GeneroDTO>(); //2da Forma
    public ICollection<CineDTO> Cines { get; set; }
    public ICollection<ActorDTO> Actores { get; set; }
}