# Curso .NET Core 3.1 Educacion IT

ActionResult es una clase que homologa los retornos que hace una API en.NET Core
Esta clase lo que permite es que ademas del valor retornado, tambien devuelve el codigo de resultado(200, 300, 400, 500)

Instalamos el CLI de EntityFramework mediante el comando 'dotnet tool install --global dotnet-ef'
https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet

Por medio de NuGet instalamos los siguientes complementos:
Microsoft.AspNetCore.Razor.Design
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools

Generamos la carpeta Entities para que contenga a las clases de nuestro proyecto
Y la carpeta Context para agregar la base de datos
Vamos a agregar nuestro contexto ApplicationDbContext al metodo ConfigureServices del Startup
(En nuestro caso, hemos agregado el contexto con el nombre "DefaultConnectionString")

Luego debemos agregar la referencia al contexto dentro de appsettings.json, agregando este fragmento dentro del json
<pre><code>
	"connectionStrings": {
    "DefaultConnectionString":  "Data Source=(localdb)\\mssqllocaldb; Initial Catalog= WebApiLibrosDB; Integrated Security = True"
  },
</code></pre>

* Una vez que llegamos a este punto, vamos a empezar con la migracion de la base.
Para esto, dentro del VS vamos a Herramientas>NuGet Package Manager>Consola; Dentro de la consola ejecutamos el comando:
<pre>
	Add-Migration [Nombre]
</pre>
Listo con esto, corremos este otro comando para generar la base de datos:
<pre>
	Update-database
</pre>

* Empezamos a escribir el código en el controlador de nuestra clase (AutorController.cs):
<pre><code>
    private readonly ApplicationDbContext context;

    public AutorController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Autor>> Get()
    {
        return context.Autores.ToList();
    }

    [HttpGet("{id}")]
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
