using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class MainSettingsController : ControllerBase
    {
        private readonly CRMDbContext _context;

        public MainSettingsController(CRMDbContext context)
        {
            _context = context;
        }

        // GET: api/MainSettings
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MainSettings>>> GetMainSettings()
        {
            return await _context.MainSettings.ToListAsync();
        }

        // GET: api/MainSettings/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<MainSettings>> GetMainSettings(int id)
        {
            var mainSettings = await _context.MainSettings.FindAsync(id);

            if (mainSettings == null)
            {
                return NotFound();
            }

            return mainSettings;
        }

        // GET: api/GetMainSettingsByCode/5
        [HttpGet("GetMainSettingByCode/{code}")]
        public async Task<ActionResult<MainSettings>> GetMainSettingByCode(int code)
        {
            var mainSetting = await _context.MainSettings.Where(s => s.codeFiled == code).FirstAsync();

            if (mainSetting == null)
            {
                return NotFound();
            }

            return mainSetting;
        }

        [HttpPut("PutMainSettingByCode/{id}/{newValue}")]
        public async Task<IActionResult> PutMainSettingByCode(int id, string newValue)
        {
            MainSettings setting = await _context.MainSettings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }
            setting.valueField = newValue;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return Ok();
        }

        // POST: api/MainSettings
        [HttpPost]
        public async Task<ActionResult<MainSettings>> PostMainSettings(MainSettings mainSettings)
        {
            _context.MainSettings.Add(mainSettings);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMainSettings", new { id = mainSettings.Id }, mainSettings);
        }

        // DELETE: api/MainSettings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMainSettings(int id)
        {
            var mainSettings = await _context.MainSettings.FindAsync(id);
            if (mainSettings == null)
            {
                return NotFound();
            }

            _context.MainSettings.Remove(mainSettings);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MainSettingsExists(int id)
        {
            return _context.MainSettings.Any(e => e.Id == id);
        }
    }
}
