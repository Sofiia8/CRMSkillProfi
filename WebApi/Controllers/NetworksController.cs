using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class NetworksController : ControllerBase
    {
        private readonly CRMDbContext _context;

        public NetworksController(CRMDbContext context)
        {
            _context = context;
        }

        // GET: api/Networks
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Network>>> GetNetworks()
        {
            return await _context.Networks.ToListAsync();
        }

        // GET: api/Networks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Network>> GetNetwork(int id)
        {
            var network = await _context.Networks.FindAsync(id);

            if (network == null)
            {
                return NotFound();
            }

            return network;
        }

        // PUT: api/Networks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNetwork(int id, Network network)
        {
            if (id != network.Id)
            {
                return BadRequest();
            }

            _context.Entry(network).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NetworkExists(id))
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

        // POST: api/Networks
        [HttpPost]
        public async Task<ActionResult<Network>> PostNetwork(Network network)
        {
            _context.Networks.Add(network);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNetwork", new { id = network.Id }, network);
        }

        // DELETE: api/Networks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNetwork(int id)
        {
            var network = await _context.Networks.FindAsync(id);
            if (network == null)
            {
                return NotFound();
            }

            _context.Networks.Remove(network);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NetworkExists(int id)
        {
            return _context.Networks.Any(e => e.Id == id);
        }
    }
}
