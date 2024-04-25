using System.ComponentModel.DataAnnotations.Schema;

namespace Sol_EFCorePeliculas.Entidades;
//Relación Manual de muchos a muchos entre Película y Actor (Creación de entidad PeliculaActor)
public class Actor
{
    public int Id { get; set; }
    /*
     * Diferencia entre un propiedad(Nombre) y un campo(_nombre)
     * Vamos a mapear no solo propiedad con columnas de una tabla de datos, sino campos con propiedades de una tabla(debemos ir al api afluente(ActorConfig) para indicar que la columna nombre va a utilizar el campo _nombre)
     */
    public string _nombre;

    /*
     * Ahora podremos colocar el nombre de diferente formato, pero siempre se convertirá en primera letra mayúscula
     * y todo lo demás en minúscula.
     */
    public string Nombre
    {
        get
        {
            return _nombre;
        }
        set
        {
            _nombre = string.Join(' ',
                value.Split(' ').Select(x => x[0].ToString().ToUpper() + x.Substring(1).ToLower()).ToArray());
        }
    }

    public string Biografio { get; set; }
    //Al hacer la migración por defecto se guarda como columna de tipo datetime2(Que guarda con hora) por lo que ponemos [date]
    //[Column(TypeName = "Date")] //También lo podemos hacer desde el api afluente
    //colocamos ? para indicar que va hacer de tipo nulo al realizar la migración
    public DateTime? FechaNacimiento { get; set; } //Con el ? puede almancenar nulo o un datetime
    public HashSet<PeliculaActor> PeliculasActores { get; set; } //Creo que acá es un list de PeliculasActores
}