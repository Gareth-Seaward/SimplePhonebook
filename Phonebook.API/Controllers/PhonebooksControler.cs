using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phonebook.API.Data;

namespace DatingApp.API.Controllers
{
  [Authorize]
  [Route("v1/[controller]")]
  [ApiController]
  public class PhoneBooksController : ControllerBase
  {
    private readonly DataContext _context;

    public PhoneBooksController(DataContext context)
    {
      _context = context;
    }
    // GET api/values
    [HttpGet]
    public async Task<IActionResult> GetPhoneBooks()
    {
      var phonebooks = await _context.Phonebooks.ToListAsync();

      return Ok(phonebooks);
    }

    // GET api/values/5
    [AllowAnonymous]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetPhonebook(int id)
    {
      var phonebook = await _context.Phonebooks.FindAsync(id);

      return Ok(phonebook); //204 if no content
    }

    // POST api/values
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
  }
}
