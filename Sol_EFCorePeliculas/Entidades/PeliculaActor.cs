namespace Sol_EFCorePeliculas.Entidades;

//Relación Manual de muchos a muchos entre Película y Actor
public class PeliculaActor
{
    public int PeliculaId { get; set; }
    public int ActorId { get; set; }
    public string Personaje { get; set; }
    public int Orden { get; set; }
    public Pelicula Pelicula { get; set; }
    public Actor Actor { get; set; }
}