using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AutoMapper;
using Biblioteca.Context;
using Biblioteca.Entities;
using Biblioteca.Helpers;
using Biblioteca.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("PermitirApiRequest")]
    public class AutorController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;

        public AutorController(ApplicationDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Autor>> Get()
        {
            return context.Autores.Include(x => x.Libros).ToList();
        }

        [HttpGet("listado")]
        [ServiceFilter(typeof(FiltroPersonalizadoDeAccion))]
        public ActionResult<IEnumerable<AutorDto>> GetListado()
        {
            var resultado = context.Autores.Include(x => x.Libros).ToList();
            var autorDto = mapper.Map<List<AutorDto>>(resultado);

            return autorDto;
        }

        [HttpGet("async")]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAsync()
        {
            return await context.Autores.Include(x => x.Libros).ToListAsync();
        }

        [HttpGet("/primer")]
        public ActionResult<Autor> GetAlRoot()
        {
            return context.Autores.FirstOrDefault();
        }

        [HttpGet("/cache")]
        [ResponseCache(Duration = 15)]
        [Authorize]
        [ServiceFilter(typeof(FiltroPersonalizadoDeAccion))]
        public ActionResult<string> GetTime()
        {
            var resultado = DateTime.Now.Second.ToString();
            return resultado;
        }

        [HttpGet("/error")]
        [ServiceFilter(typeof(FiltroDeExcepcion))]
        public ActionResult<string> GetError()
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}", Name = "ObtenerAutor")]
        public ActionResult<Autor> Get(int id)
        {
            var result = context.Autores.FirstOrDefault(autor => autor.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost]
        public ActionResult Post([FromBody] Autor autor)
        {
            context.Autores.Add(autor);
            context.SaveChanges();

            return new CreatedAtRouteResult("ObtenerAutor", new { id = autor.Id }, autor);
        }

        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] Autor value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }

            context.Entry(value).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
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
    }
}
