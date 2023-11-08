using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Sol_EFCorePeliculas.Entidades;
using Sol_EFCorePeliculas.Entidades.Configuraciones;
using Sol_EFCorePeliculas.Entidades.Seddiing;

namespace Sol_EFCorePeliculas;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    //Para configurar el comportamiento por defecto que hace entity framework al mapear los campos en la base de datos(de un string cree una columna de tipo nvarchar(max))
    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        //Configurando para que al hacer migraciones el valor por defecto sea date y no datetime2, ya no sería necesario configurando en el api afluente
        configurationBuilder.Properties<DateTime>().HaveColumnType("date"); 
    }

    //Api Afluente o Api fluida(En el api afluente se puede hacer cosas que no se pueden hacer por atributo mediante DataAnnotations)
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        /*
         Para ordenar nuestro apiafluente creamos una carpeta aparte llamada configuraciones y ahí colocamos nuestras personalizaciones
        //Llave identificador es una llave primaria
        modelBuilder.Entity<Genero>().HasKey(prop => prop.Identificador);
        //Si no personalizo el la propiedad se genera con: "nvarchar(max)" el cuál significa hasta 2 gb de almacenamiento
        modelBuilder.Entity<Genero>().Property(prop => prop.Nombre)
            .HasColumnName("NombreGenero")
            .HasMaxLength(150)
            .IsRequired();
        modelBuilder.Entity<Genero>().ToTable(name: "TablaGeneros", schema: "Peliculas");
        */
        
        //Invocar nuestro nuevo GeneroConfig dónde están todas nuestras personalizaciones
        //modelBuilder.ApplyConfiguration(new GeneroConfig()); //1era forma

        //2da Forma Tomando todas las IEntityTypeConfiguration de una(Assembly: escanea el proyecto en el que estamos y toma todas las clases que heredan de IEntityTypeConfiguration y aplícalas en nuestro api afluente)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());//Assembly.GetExecutingAssembly(): le indicamos que el assembly se está ejecutando
        SeedingModuloConsulta.Seed(modelBuilder); //Le estamos pasando el SeedingModuloConsulta para cargar datos de pruebas

        /*
        modelBuilder.Entity<Actor>().Property(prop => prop.Nombre)
            .HasMaxLength(150)
            .IsRequired();
        modelBuilder.Entity<Actor>().Property(prop => prop.FechaNacimiento)
            .HasColumnType("Date");
        */

        /*
        modelBuilder.Entity<Cine>().Property(prop => prop.Nombre)
            .HasMaxLength(150)
            .IsRequired();
        */

        /*
        modelBuilder.Entity<Pelicula>().Property(prop => prop.Titulo)
            .HasMaxLength(250)
            .IsRequired();
        modelBuilder.Entity<Pelicula>().Property(prop => prop.FechaEstreno)
            .HasColumnType("date");
        //Las url no utilizan todos los caracteres , utiliza los caracteres llamados ASCII
        modelBuilder.Entity<Pelicula>().Property(prop => prop.PosterURL)
            .HasMaxLength(500)
            //Se refiere a los caracteres que vamos a aceptar en este campo(Si deseamos permitir caracteres como la ñ o letras árabes incluso emojis), como no necesitamos almacenar ese tipo de caracteres colocamos false
            .IsUnicode(false);  //Colocamos false netamente por un ahorro de memoria ram, pero por ejemplo sería true en un campo comentario, dónde el usuario puede colocar cualquier cosa
        */

        /*
        modelBuilder.Entity<CineOferta>().Property(prop => prop.PorcentajeDescuento)
            .HasPrecision(precision: 5, scale: 2);
        modelBuilder.Entity<CineOferta>().Property(prop => prop.FechaInicio)
            .HasColumnType("date");
        modelBuilder.Entity<CineOferta>().Property(prop => prop.FechaFin)
            .HasColumnType("date");
        */

        /*
        modelBuilder.Entity<SalaDeCine>().Property(prop => prop.Precio)
            .HasPrecision(precision: 9, scale: 2);
        modelBuilder.Entity<SalaDeCine>().Property(prop => prop.TipoSalaDeCine)
            //.HasDefaultValueSql("GetDate()") //La diferencia entre HasDefaultValueSql y HasDefaultValue es que al primero le pasamos una expresión sql como valor por defecto
            .HasDefaultValue(TipoSalaDeCine.DosDimensiones); //El valor por defecto en la migracion seria 1 (Revisar el enum)
        */

        /*
        modelBuilder.Entity<PeliculaActor>().HasKey(prop =>
            new { prop.PeliculaId, prop.ActorId });
        modelBuilder.Entity<PeliculaActor>().Property(prop => prop.Personaje)
            .HasMaxLength(150);
        */
    }

    public DbSet<Genero> Generos { get; set; }
    public DbSet<Actor> Actores { get; set; }
    public DbSet<Cine> Cines { get; set; }
    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<CineOferta> CinesOfertas { get; set; }
    public DbSet<SalaDeCine> SalaDeCines { get; set; }
    public DbSet<PeliculaActor> PeliculasActores { get; set; }
}