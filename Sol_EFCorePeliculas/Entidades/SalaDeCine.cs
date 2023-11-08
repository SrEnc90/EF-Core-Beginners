namespace Sol_EFCorePeliculas.Entidades;

//Relacion de muchos a uno con respecto de Cine(Un cine puede tener varios precios según el tipo de sala)
//Relación simple o automática de muchos a muchos con la entidad Película
public class SalaDeCine
{
    public int Id { get; set; }

    public TipoSalaDeCine TipoSalaDeCine { get; set; }    
    // [Precision(precision:9,scale:2)]//Precision: Es la cantidad de enteros y scale es la cantidad de dígitos decimales
    public decimal Precio { get; set; }//Al realizar la migracion, se va a crear una columna decimal(18,2), por lo que podemos configurar sus parámetros gracias al api afluente
    public int CineId { get; set; }
    public Cine Cine { get; set; }
    public HashSet<Pelicula> Peliculas { get; set; }
}