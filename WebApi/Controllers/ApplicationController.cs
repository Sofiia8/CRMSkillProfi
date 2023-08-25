using System;
using System.Collections.Generic;
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
    public class ApplicationController : ControllerBase
    {
        private readonly CRMDbContext _context;

        public ApplicationController(CRMDbContext context)
        {
            _context = context;
        }

        // GET: api/Application
        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApplicationView>>> GetApplications()
        {
            var applications = _context.Applications.Join(_context.ApplicationStates,
                                                    ap => ap.ApplicationStatusId,
                                                    st => st.Id,
                                                    (ap, st) => new ApplicationView
                                                    {
                                                        Id = ap.Id,
                                                        Datetime = ap.Datetime,
                                                        Name = ap.Name,
                                                        Email = ap.Email,
                                                        Description = ap.Description,
                                                        Status = st.Status
                                                    });
            return await applications.ToListAsync();
        }

        // GET: api/Application/GetFromToDates?dateTimeFrom=2023-06-16T11:06:30&dateTimeTo=2023-06-17T11:06:30
        [Authorize(Roles = "admin")]
        [HttpGet("GetFromToDates")]
        public async Task<ActionResult<IEnumerable<ApplicationView>>> GetFromToDates([FromQuery] DateTime dateTimeFrom, [FromQuery] DateTime dateTimeTo)
        {
            return await _context.Applications.Join(_context.ApplicationStates,
                                                    ap => ap.ApplicationStatusId,
                                                    st => st.Id,
                                                    (ap, st) => new ApplicationView
                                                    {
                                                        Id = ap.Id,
                                                        Datetime = ap.Datetime,
                                                        Name = ap.Name,
                                                        Email = ap.Email,
                                                        Description = ap.Description,
                                                        Status = st.Status
                                                    }).
                    Where(ap => ap.Datetime >= dateTimeFrom && ap.Datetime <= dateTimeTo.Date.Add(new TimeSpan(23, 59, 59)))
                    .ToListAsync();
        }


        // GET: api/Application/5
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationView>> GetApplication(int id)
        {
            var ap = await _context.Applications.Join(_context.ApplicationStates,
                                                    ap => ap.ApplicationStatusId,
                                                    st => st.Id,
                                                    (ap, st) => new ApplicationView
                                                    {
                                                        Id = ap.Id,
                                                        Datetime = ap.Datetime,
                                                        Name = ap.Name,
                                                        Email = ap.Email,
                                                        Description = ap.Description,
                                                        Status = st.Status
                                                    })
                                                    .Where(ap => ap.Id == id).FirstAsync();

            if (ap == null)
            {
                return NotFound();
            }

            return ap;
        }

        // PUT: api/Application/5
        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApplication(int id, Application application)
        {
            if (id != application.Id)
            {
                return BadRequest();
            }

            _context.Entry(application).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApplicationExists(id))
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

        [Authorize(Roles = "admin")]
        [HttpPut("PutStatusApplication/{id}/{status}")]
        public async Task<IActionResult> PutStatusApplication(int id, string status)
        {
            var application = await _context.Applications.FindAsync(id);

            if (application == null)
            {
                return NotFound();
            }

            var newStatus = await _context.ApplicationStates.Where(st => st.Status == status).FirstAsync();
            application.ApplicationStatusId = newStatus.Id;

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


        // POST: api/Application
        [HttpPost]
        public async Task<ActionResult<Application>> PostApplication(ApplicationView applicationView)
        {
            var application = new Application
            {
                Datetime = applicationView.Datetime,
                Name = applicationView.Name,
                Email = applicationView.Email,
                Description = applicationView.Description,
                ApplicationStatusId = 1
            };
            _context.Applications.Add(application);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }

            return CreatedAtAction("GetApplication", new { id = application.Id }, application);
        }

        // DELETE: api/Application/5
        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [Authorize(Roles = "admin")]
        private bool ApplicationExists(int id)
        {
            return _context.Applications.Any(e => e.Id == id);
        }
    }
}
