using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Sol_EFCorePeliculas;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllers();

/*
 * Para eliminar el error de A possible object cycle was detected al hacer el eager loading (ocurre
 * cuándo una clase tiene una propiedad de navegación a una segunda clase, pero la segunda clase también
 * tiene una propiedad de navegación a la primera clase). Hay dos formas de arreglarlo:
 * 1) Mediante un DTO
 * 2) Mediante el program.cs file editamos las opciones del controlador
 */
builder.Services.AddControllers().AddJsonOptions(opciones =>
    opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//Inyección del ConnectionString
var connectionString = builder.Configuration.GetConnectionString("ConexionSQL");
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    {
        opciones.UseSqlServer(connectionString,
            sqlserver => sqlserver.UseNetTopologySuite()); //Para usar netTopologySuite
        opciones.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); //Haciendo que las consulta select sea de solo lectura pero de manera global

    }
);

//AUTOMAPPER
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();