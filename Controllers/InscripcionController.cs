using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using proyectoFinal.Data;
using proyectoFinal.Models;

namespace proyectoFinal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InscripcionController : ControllerBase
    {
        private readonly projectContext _context;

        public InscripcionController(projectContext context)
        {
            _context = context;
        }

        // GET: api/Inscripcion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inscripcion>>> GetInscripciones()
        {
            return await _context.Inscripciones.ToListAsync();
        }

        // GET: api/Inscripcion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Inscripcion>> GetInscripcion(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);

            if (inscripcion == null)
            {
                return NotFound();
            }

            return inscripcion;
        }

           // GET: api/Inscripcion/evento/5
        [HttpGet("evento/{id}")]
        public async Task<IEnumerable<dynamic>> GetInscripcionesEvento(int id)
        {
            var inscripcion = await _context.Inscripciones.Where(w=>w.eventoId==id).Select(s=> new {s.usuario.username}).ToListAsync();


            return inscripcion;
        }


        // GET: api/Inscripcion/Usuario/5
        [HttpGet("Usuario/{id}")]
        public async Task<IEnumerable<dynamic>> GetInscripcionesUsuario(int id)
        {
            var inscripciones = await _context.Inscripciones.Where(w=>w.usuarioId==id).Select(s=>new {s,s.evento.evento}).ToListAsync();

            return inscripciones;
        }

        // GET: api/Inscripcion/HypeUp/5
        [HttpGet("HypeUp/{id}")]
        public async Task<IActionResult> HypeUp(int id)
        {
            var inscripcionAsync = await _context.Inscripciones.Where(w=>w.inscripcionId==id).ToListAsync();

            var eventoAsync = await _context.Eventos.Where(w=>w.eventoId==inscripcionAsync[0].eventoId).ToListAsync();
   
            inscripcionAsync[0].valoracion = 1;

            eventoAsync[0].popularidad++;
            
            _context.Entry(inscripcionAsync[0]).State = EntityState.Modified;

            _context.Entry(eventoAsync[0]).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscripcionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            

            return NoContent();
        }

        // GET: api/Inscripcion/HypeDown/5
        [HttpGet("HypeDown/{id}")]
        public async Task<IActionResult> HypeDown(int id)
        {
            var inscripcionAsync = await _context.Inscripciones.Where(w=>w.inscripcionId==id).ToListAsync();

            var eventoAsync = await _context.Eventos.Where(w=>w.eventoId==inscripcionAsync[0].eventoId).ToListAsync();
   
            inscripcionAsync[0].valoracion = 0;
            
            eventoAsync[0].popularidad--;
            
            _context.Entry(inscripcionAsync[0]).State = EntityState.Modified;

            _context.Entry(eventoAsync[0]).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscripcionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            

            return NoContent();
        }

        // PUT: api/Inscripcion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInscripcion(int id, Inscripcion inscripcion)
        {
            if (id != inscripcion.inscripcionId)
            {
                return BadRequest();
            }

            _context.Entry(inscripcion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InscripcionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Inscripcion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Inscripcion>> PostInscripcion(Inscripcion inscripcion)
        {
            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInscripcion", new { id = inscripcion.inscripcionId }, inscripcion);
        }

        // DELETE: api/Inscripcion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInscripcion(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            _context.Inscripciones.Remove(inscripcion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InscripcionExists(int id)
        {
            return _context.Inscripciones.Any(e => e.inscripcionId == id);
        }
    }
}
