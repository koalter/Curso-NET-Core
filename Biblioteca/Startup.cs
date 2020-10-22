using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Biblioteca.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Biblioteca.Helpers;
using AutoMapper;
using Biblioteca.Models;
using Biblioteca.Entities;

namespace Biblioteca
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddCors();
            services.AddCors(options => options.AddPolicy("PermitirApiRequest", builder => builder.WithOrigins("http://www.apirequest.io").WithMethods("GET", "POST").AllowAnyHeader()));
            services.AddAutoMapper(configuration =>
            {
                //configuration.CreateMap(typeof(Libro), typeof(LibroDto));
                configuration.CreateMap<Libro, LibroDto>();
            }, typeof(Startup));
            services.AddAutoMapper(configuration =>
            {
                //configuration.CreateMap(typeof(Autor), typeof(AutorDto));
                configuration.CreateMap<Autor, AutorDto>();
            }, typeof(Startup));

            services.AddScoped<FiltroPersonalizadoDeAccion>();
            services.AddResponseCaching();

            // Agregamos el contexto de la base de datos
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnectionString")));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddControllers()
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore); 
                // Agregamos la referencia al newtonsoft para evitar la referencia ciclica
            services.AddMvc(Options => Options.Filters.Add(new FiltroDeExcepcion())); // Agregamos la referencia al filtro de excepcion
            

            //services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseResponseCaching();

            app.UseAuthentication();

            app.UseAuthorization();

            //app.UseCors(builder => builder.WithOrigins("http://www.apirequest.io"));
            //app.UseCors(builder => builder.WithOrigins("http://www.apirequest.io").WithMethods("GET").AllowAnyHeader());
            app.UseCors(); // Se puede utilizar asi en conjunto con la linea 36

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
