using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreWebApi6.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApi6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeaponController : ControllerBase
    {
        private readonly WeaponContext _dbContext;

        public WeaponController(WeaponContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Weapon
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Weapon>>> GetWeapons()
        {
            if (_dbContext.Weapons == null)
            {
                return NotFound();
            }

            return await _dbContext.Weapons.ToListAsync();
        }

        // GET: api/Weapon/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Weapon>> GetWeapon(int id)
        {
            var weapon = await _dbContext.Weapons.FindAsync(id);

            if (weapon == null)
            {
                return NotFound();
            }

            return weapon;
        }

        // POST: api/Weapon
        [HttpPost]
        public async Task<ActionResult<Weapon>> PostWeapon(Weapon weapon)
        {
            _dbContext.Weapons.Add(weapon);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction("GetWeapon", new { id = weapon.Id }, weapon);
        }

        // PUT: api/Weapon/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeapon(int id, Weapon weapon)
        {
            if (id != weapon.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(weapon).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WeaponExists(id))
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

        // DELETE: api/Weapon/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeapon(int id)
        {
            var weapon = await _dbContext.Weapons.FindAsync(id);
            if (weapon == null)
            {
                return NotFound();
            }

            _dbContext.Weapons.Remove(weapon);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        private bool WeaponExists(int id)
        {
            return _dbContext.Weapons.Any(e => e.Id == id);
        }
    }
}
