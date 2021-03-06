﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Biblioteca.Context;
using Biblioteca.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutorController : ControllerBase
    {
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
