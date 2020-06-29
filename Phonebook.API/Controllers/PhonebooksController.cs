using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Phonebook.API.ActionFilters;
using Phonebook.API.Constants;
using Phonebook.API.Data;
using Phonebook.API.Dtos;
using Models = Phonebook.API.Models;

namespace DatingApp.API.Controllers
{

  [Authorize]
  [Route("v1/")]
  [ApiController]
  public class PhoneBooksController : ControllerBase
  {
    private readonly IPhonebookRepository _phonebookRepo;
    private readonly IMapper _mapper;

    public PhoneBooksController(IPhonebookRepository phonebookRepo,
    IMapper mapper)
    {
      _phonebookRepo = phonebookRepo;
      _mapper = mapper;
    }

    [HttpGet("users/{id}/[controller]")]
    [ServiceFilter(typeof(ValidateUSerIdAttribute))]
    public async Task<IActionResult> GetPhonebook(int id)
    {
      var phonebook = await GetPhonebookFromRepo(id);

      var phonebookForResponse = _mapper.Map<PhonebookForResponseDto>(phonebook);
      return Ok(phonebookForResponse); //204 if no content
    }

    

    // PUT api/values/5
    [HttpPut("users/{id}/[controller]")]
    [ServiceFilter(typeof(ValidateUSerIdAttribute))]
    public async Task<IActionResult> UpdatePhoneBook(int id, [FromBody] PhonebookForUpdateDto phonebookForupdate)
    {
      var phonebook = await GetPhonebookFromRepo(id);

      phonebook.Name = phonebookForupdate.Name;

      await _phonebookRepo.SaveAsync();
      return Ok();
    }

    private async Task<Models.Phonebook> GetPhonebookFromRepo(int id)
    {
      var phonebook = HttpContext.Items[HttpContextConstants.PhonebookItem] as Models.Phonebook;
      if (phonebook is null)
        phonebook = await _phonebookRepo.GetPhonebook(id);
      return phonebook;
    }
  }
}
