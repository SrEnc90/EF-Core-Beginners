namespace Sol_EFCorePeliculas.Entidades;

public class Pelicula
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public bool EnCartelera { get; set; }
    public DateTime FechaEstreno { get; set; }
    public string PosterURL { get; set; }
    
    //public HashSet<Genero> Generos { get; set; } //El HashSet no garantiza el orden de los elementos ni permite valores duplicados. Por lo que si vamos a ordernar es mejor utilizar el List
    public List<Genero> Generos { get; set; }
    // public HashSet<SalaDeCine> SalaDeCines { get; set; } El HashSet no es IEnumerable no me permite hacer forEach con el método Post de PeliculasController
    public List<SalaDeCine> SalaDeCines { get; set; }
    //Mismo problema que HashSet<SalaDeCine>
    //public HashSet<PeliculaActor> PeliculasActores { get; set; } //Relación muchos a muchos, Tabla intermedia (PeliculaActor)
    public List<PeliculaActor> PeliculasActores { get; set; } //Relación muchos a muchos, Tabla intermedia (PeliculaActor)
}