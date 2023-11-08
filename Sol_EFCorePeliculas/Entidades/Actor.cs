using System.ComponentModel.DataAnnotations.Schema;

namespace Sol_EFCorePeliculas.Entidades;
//Relación Manual de muchos a muchos entre Película y Actor (Creación de entidad PeliculaActor)
public class Actor
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Biografio { get; set; }
    //Al hacer la migración por defecto se guarda como columna de tipo datetime2(Que guarda con hora) por lo que ponemos [date]
    //[Column(TypeName = "Date")] //También lo podemos hacer desde el api afluente
    //colocamos ? para indicar que va hacer de tipo nulo al realizar la migración
    public DateTime? FechaNacimiento { get; set; } //Con el ? puede almancenar nulo o un datetime
    public HashSet<PeliculaActor> PeliculasActores { get; set; } //Creo que acá es un list de PeliculasActores
}