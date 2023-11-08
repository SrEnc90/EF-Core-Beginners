namespace Sol_EFCorePeliculas.DTOs;

public class PeliculaCreacionDTO
{
    public string Titulo { get; set; }
    public bool EnCartelera { get; set; }
    public DateTime FechaEstreno { get; set; }
    public List<int> GenerosId { get; set; }
    public List<int> SalasDeCineId { get; set; }
    public List<PeliculaActorCreacionDTO> PeliculasActores { get; set; }
}