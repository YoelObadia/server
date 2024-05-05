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
        // Database context
        private readonly WeaponContext _dbContext;

        // Constructor
        public WeaponController(WeaponContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/Weapon
        /// <summary>
        /// Get all weapons from the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Weapon>>> GetWeapons()
        {
            // If there are no weapons
            if (_dbContext.Weapons == null)
            {
                return NotFound();
            }

            // Return all weapons
            return await _dbContext.Weapons.ToListAsync();
        }

        // GET: api/Weapon/{id}
        /// <summary>
        /// Get a weapon by id shared in parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Weapon>> GetWeapon(int id)
        {
            // Find the weapon by id
            var weapon = await _dbContext.Weapons.FindAsync(id);

            // If the weapon is not found
            if (weapon == null)
            {
                return NotFound();
            }

            // Return the weapon
            return weapon;
        }

        // POST: api/Weapon
        /// <summary>
        /// Add a weapon to the database
        /// </summary>
        /// <param name="weapon"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Weapon>> PostWeapon(Weapon weapon)
        {
            // Add the weapon to the database
            _dbContext.Weapons.Add(weapon);

            // Save the changes
            await _dbContext.SaveChangesAsync();

            // Return the weapon
            return CreatedAtAction("GetWeapon", new { id = weapon.Id }, weapon);
        }

        // PUT: api/Weapon/{id}
        /// <summary>
        /// Update a weapon in the database by id shared in parameter
        /// </summary>
        /// <param name="id"></param>
        /// <param name="weapon"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWeapon(int id, Weapon weapon)
        {
            // If the id does not match the weapon id 
            if (id != weapon.Id) 
            {
                return BadRequest();
            }

            // Change the state of the weapon
            _dbContext.Entry(weapon).State = EntityState.Modified;

            // Save the changes
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
        /// <summary>
        /// Delete a weapon from tha database by id shared in parameter
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeapon(int id)
        {
            // Find the weapon by id
            var weapon = await _dbContext.Weapons.FindAsync(id);

            // If the weapon is not found
            if (weapon == null)
            {
                return NotFound();
            }

            // Remove the weapon
            _dbContext.Weapons.Remove(weapon);

            // Save the changes
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }

        
        /// <summary>
        /// Check if a weapon exists in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool WeaponExists(int id)
        {
            return _dbContext.Weapons.Any(e => e.Id == id);
        }
    }
}
