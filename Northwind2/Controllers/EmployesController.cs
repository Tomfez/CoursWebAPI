using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind2.Data;
using Northwind2.Entities;
using Northwind2.Services;

namespace Northwind2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployesController : ControllerBase
    {
        private readonly IServiceEmployes _service;

        public EmployesController(IServiceEmployes service)
        {
            _service = service;
        }

        // GET: api/Employes
        /// <summary>
        /// Renvoie la liste des employés
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employe>>> GetEmployes()
        {
            List<Employe> employes = await _service.ObtenirEmployes();
            return Ok(employes);
        }

        // GET: api/Employes/5
        /// <summary>
        /// Retourne un employé selon son id
        /// </summary>
        /// <param name="id">identifiant de l'employé</param>
        /// <response code="200">renvoie l'employé d'id donné</response>
        /// <response code="404">employé non trouvé</response>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employe>> GetEmploye(int id)
        {
            var employe = await _service.ObtenirEmploye(id);

            if (employe == null)
            {
                return NotFound();
            }

            return Ok(employe);
        }

        [HttpGet("/api/Regions/{id}")]
        [Authorize(Policy = "GérerEmployés")]
        public async Task<ActionResult<Region>> GetRegion(int id)
        {
            Region? region = await _service.ObtenirRegion(id);

            if (region == null) return NotFound();

            return Ok(region);
        }

        //// PUT: api/Employes/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutEmploye(int id, Employe employe)
        //{
        //    if (id != employe.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(employe).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!EmployeExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/Employes
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Employe>> PostEmploye(Employe employe)
        //{
        //    _context.Employes.Add(employe);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetEmploye", new { id = employe.Id }, employe);
        //}

        //// DELETE: api/Employes/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteEmploye(int id)
        //{
        //    var employe = await _context.Employes.FindAsync(id);
        //    if (employe == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Employes.Remove(employe);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool EmployeExists(int id)
        //{
        //    return _context.Employes.Any(e => e.Id == id);
        //}
    }
}
