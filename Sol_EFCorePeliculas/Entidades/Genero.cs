//using System.ComponentModel.DataAnnotations;

namespace Sol_EFCorePeliculas.Entidades;

// [Table("TablaGeneros", Schema = "peliculas")]
/*
 * Relación automática de muchos a muchos con la entidad película(no vamos a tener el control de la tabla intermedia). Se usa para relaciomes simples
 */
public class Genero
{
    public int Identificador { get; set; }
    
    //[StringLength(150)]//con esto genero un nvarchar(150) en mi bbdd
    //[MaxLength] //Parecido al de arriba
    // [Required]
    // [Column("NombreGenero")]
    public string Nombre { get; set; }
    public HashSet<Pelicula> Peliculas { get; set; }
}