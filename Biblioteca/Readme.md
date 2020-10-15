# Curso .NET Core 3.1 Educacion IT

ActionResult es una clase que homologa los retornos que hace una API en.NET Core
Esta clase lo que permite es que ademas del valor retornado, tambien devuelve el codigo de resultado(200, 300, 400, 500)

Instalamos el CLI de EntityFramework mediante el comando 'dotnet tool install --global dotnet-ef'
https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

Por medio de NuGet instalamos los siguientes complementos:
Microsoft.AspNetCore.Razor.Design
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools

1. Generamos la carpeta Entities para que contenga a las clases de nuestro proyecto
2. Y la carpeta Context para agregar la base de datos, incluyendo nuestras entidades: 
<pre><code>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    public DbSet<Autor> Autores { get; set; }
}
</code></pre>
3. Vamos a agregar nuestro contexto ApplicationDbContext al metodo ConfigureServices del Startup
(En nuestro caso, hemos agregado el contexto con el nombre "DefaultConnectionString")

4. Luego debemos agregar la referencia al contexto dentro de appsettings.json, agregando este fragmento dentro del json
<pre><code>
	"connectionStrings": {
    "DefaultConnectionString":  "Data Source=(localdb)\\mssqllocaldb; Initial Catalog= WebApiLibrosDB; Integrated Security = True"
  },
</code></pre>

5. Una vez que llegamos a este punto, vamos a empezar con la migracion de la base.
Para esto, dentro del VS vamos a Herramientas>NuGet Package Manager>Consola; Dentro de la consola ejecutamos el comando:
<pre>
	Add-Migration [Nombre]
</pre>
Listo con esto, corremos este otro comando para generar la base de datos:
<pre>
	Update-database
</pre>

6. Empezamos a escribir el código en el controlador de nuestra clase (AutorController.cs):
<pre><code>
private readonly ApplicationDbContext context;

public AutorController(ApplicationDbContext context)
{
    this.context = context;
}

[HttpGet] // Con este decorador indicamos que el metodo es de tipo GET
public ActionResult<IEnumerable<Autor>> Get()
{
    return context.Autores.ToList();
}

[HttpGet("{id}")] // El parametro incluido es el formato en que se leera desde la url
public ActionResult<Autor> Get(int id)
{
    var result = context.Autores.FirstOrDefault(autor => autor.Id == id);

    if (result == null)
    {
        return NotFound();
    }

    return result;
}
</code></pre>

* Ya estamos listos para correr la aplicacion. 
Vamos primero a las Propiedades>Depurar y generamos el routeo a nuestro controlador (en este caso, api/autor)

--

* Podemos hacer que una propiedad de una entidad sea not-null agregando el decorador [Required] por encima de la propiedad.
Para esto, primero debemos incluir la libreria System.ComponentModel.DataAnnotations;
<pre><code>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations; // Agregamos la libreria

namespace Biblioteca.Entities
{
    public class Autor
    {
        public int Id { get; set; }

        [Required] // Añadimos el decorador
        public string Nombre { get; set; }
    }
}
</code></pre>

* Metodo POST
<pre><code>
[HttpGet("{id}", Name = "ObtenerAutor")] // "ObtenerAutor" es un alias, que utilizaremos en el metodo POST debajo
public ActionResult<Autor> Get(int id)
{
    var result = context.Autores.FirstOrDefault(autor => autor.Id == id);

    if (result == null)
    {
        return NotFound();
    }

    return result;
}

[HttpPost] // El decorador nos indica que el metodo es de tipo POST
public ActionResult Post([FromBody] Autor autor) // El decorador [FromBody] hace que obtenga los datos desde el cuerpo del request
{
    context.Autores.Add(autor);
    context.SaveChanges();

    return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autor); // Este metodo llama al metodo del alias señalado
}
</code></pre>

* Metodo PUT
<pre><code>
[HttpPut("{id}")] // El decorador nos indica que el metodo es de tipo PUT, y le agregamos el parametro correspondiente para el id
public ActionResult Put(int id, [FromBody] Autor value)
{
    if (id != value.Id)
    {
        return BadRequest();
    }

    context.Entry(value).State = Microsoft.EntityFrameworkCore.EntityState.Modified; // El metodo Entry se encarga de hacer la modificacion
    context.SaveChanges();

    return Ok();
}
</code></pre>

* Metodo DELETE
<pre><code>
[HttpDelete("{id}")] // El decorador nos indica que el metodo es de tipo DELETE, y le agregamos una vez mas el parametro correspondiente para el id
public ActionResult<Autor> Delete(int id)
{
    var resultado = context.Autores.FirstOrDefault(autor => autor.Id == id);

    if (resultado == null)
    {
        return NotFound();
    }

    context.Autores.Remove(resultado);
    context.SaveChanges();

    return resultado;
}
</code></pre>

Pasemos ahora a generar nuestra segunda entidad Libro para hacer las uniones con nuestros autores, siguiendo los mismos pasos (podemos ignorar el 3 y el 4).

--

Si al decorador del metodo lo iniciamos con una barra, hacemos que ignore todo el routeo del controlador para que inicie desde la raiz. Por ejemplo asi
<pre><code>
[HttpGet("/primer")]
</code></pre>

# Error de referencia ciclica

Debemos instalar el paquete NuGet Microsoft.AspNetCore.Mvc.NewtonsoftJson

# Llamadas asincronicas

Agregamos al metodo que necesitemos que sea asincrono la palabra reservada async, que devuelve un objeto Task, y a su vez al retorno le agregamos await y que llame a un metodo asincrono.
Por ejemplo:
<pre><code>
public async Task<ActionResult<IEnumerable<Autor>>> GetAsync() // el metodo incluye async y devuelve un objeto Task<T>
{
    return await context.Autores.Include(x => x.Libros).ToListAsync(); // el retorno pide esperar al resultado, y en lugar de llamar al ToList() llama a ToListAsync()
}
</code></pre>

# Otros decoradores para atributos de las entidades

<code>
[Range(18, 120)] // agrega un rango numeral minimo y maximo
public int Edad { get; set; }
[CreditCard] // le da formato de tarjeta de credito
public string TarjetaCredito { get; set; }
[EmailAddress] // formato para direcciones de mail
public string Email { get; set; }
[Url] // formato para direcciones web
public string Url { get; set; }
</code>

* Validadores

1. A la entidad a la que queremos agregar un validador le hacemos que herede la interfaz IValidatableObject, que debera implementar su metodo correspondiente

<code>
public class Autor : IValidatableObject
{
    [PrimeraLetraMayuscula]
    [StringLength(30, ErrorMessage = "El nombre del autor puede tener hasta {1} caracteres")]
    public string Nombre { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        throw new NotImplementedException();
    }
}
</code>

--

services.AddTransient(); --> Por lo general no es buena practica utilizar este, porque siempre genera una nueva instancia
services.AddSingleton();

# Middlewares

Son aplicaciones que se ejecutan en el medio de nuestro WebAPI y el cliente, por medio de pipelines, principalmente para hacer validaciones y autenticaciones.

* Cache

En el Startup agregamos por un lado, en ConfigureServices() la llamada a services.AddResponseCaching(); 

En cuanto al Configure, lo utilizamos como app.UseResponseCaching();

Ponemos en práctica el cacheo con un nuevo endpoint, similar al que tenemos debajo (probar con IE):

<code>
[HttpGet("/cache")]
[ResponseCache(Duration = 15)]
public ActionResult<string> GetTime()
{
    var resultado = DateTime.Now.Second.ToString();
    return resultado;
}
</code>

* Filtros

1. Filtros de autorización
    - Microsoft.AspNetCore.Authentication.JwtBearer
2. Filtros de recurso
3. Filtros de acción
    - Microsoft.AspNetCore.Mvc.Filters --> IActionFilter
    (Ver Biblioteca/Helpers/FiltroPersonalizadoDeAccion.cs)
4. Filtros de excepción
    - Microsoft.AspNetCore.Mvc.Filters --> ExceptionFilterAttribute
5. Filtros de resultado

# Trabajo con recursos


