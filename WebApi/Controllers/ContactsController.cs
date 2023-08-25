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
    [Authorize(Roles = "admin")]
    public class ContactsController : ControllerBase
    {
        private readonly CRMDbContext _context;

        public ContactsController(CRMDbContext context)
        {
            _context = context;
        }

        // GET: api/Contacts
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Contacts>>> GetContacts()
        {
            return await _context.Contacts.ToListAsync();
        }

        // GET: api/Contacts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contacts>> GetContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        [HttpGet("GetContactByCode/{code}")]
        public async Task<ActionResult<Contacts>> GetContactByCode(int code)
        {
            var contact = await _context.Contacts.Where(s => s.codeField == code).FirstAsync();

            if (contact == null)
            {
                return NotFound();
            }

            return contact;
        }

        [HttpPut("PutContactById/{id}/{newValue}")]
        public async Task<IActionResult> PutContactById(int id, string newValue)
        {
            Contacts contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return NotFound();
            }
            contact.valueField = newValue;

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
    }
}
