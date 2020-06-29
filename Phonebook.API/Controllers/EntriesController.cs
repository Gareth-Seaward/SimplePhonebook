using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phonebook.API.ActionFilters;
using Phonebook.API.Constants;
using Phonebook.API.Data;
using Phonebook.API.Dtos;
using Phonebook.API.Helpers;
using Phonebook.API.Models;

namespace Phonebook.API.Controllers
{
  [Authorize]
  [Route("v1/")]
  [ApiController]
  public class EntriesController : ControllerBase
  {
    private readonly IEntriesRepository _entriesRepo;
    private readonly IMapper _mapper;

    public EntriesController(IEntriesRepository entriesRepo, IMapper mapper)
    {
      _entriesRepo = entriesRepo;
      _mapper = mapper;
    }

    [HttpGet("phonebooks/{id}/entries")]
    [ServiceFilter(typeof(ValidatePhonebookIdAttribute))]
    public async Task<IActionResult> GetEntries(int id, [FromQuery] EntryParams entryParams)
    {
      var entries = await _entriesRepo.GetEntries(id, entryParams);

      var entriesForResponse = _mapper.Map<IEnumerable<EntryForResponseDto>>(entries);

      Response.AddPaginations(entries.CurrentPage, entries.PageSize, entries.TotalCount, entries.TotalPages);
      return Ok(entriesForResponse);
    }

    [HttpGet("phonebooks/{id}/entries/{entryid}", Name = "GetEntry")]
    [ServiceFilter(typeof(ValidatePhonebookIdAttribute))]
    [ServiceFilter(typeof(ValidateEntryIdAttribute))]
    public async Task<IActionResult> GetEntry(int id, int entryid)
    {
      var entryForResponse = await Task.Run(() =>
      {
        var entry = HttpContext.Items[HttpContextConstants.EntryItem] as Models.Entry;
        return _mapper.Map<Models.Entry, EntryForResponseDto>(entry);
      });

      return Ok(entryForResponse);

    }

    [HttpPut("phonebooks/{id}/entries/{entryid}")]
    [ServiceFilter(typeof(ValidatePhonebookIdAttribute))]
    [ServiceFilter(typeof(ValidateEntryIdAttribute))]
    public async Task<IActionResult> UpdateEntry(int id, int entryid, EntryForUpdateDto entryForUpdate)
    {
      var entry = HttpContext.Items[HttpContextConstants.EntryItem] as Models.Entry;
      _mapper.Map<EntryForUpdateDto, Models.Entry>(entryForUpdate, entry);

      await _entriesRepo.SaveAsync();
      return Ok();
    }

    [HttpPost("phonebooks/{id}/entries/")]
    [ServiceFilter(typeof(ValidatePhonebookIdAttribute))]
    public async Task<IActionResult> CreateEntry(int id, EntryForUpdateDto entryForUpdate)
    {
      var Phonebook = HttpContext.Items[HttpContextConstants.PhonebookItem] as Models.Phonebook;
      var entryForCreate = _mapper.Map<EntryForUpdateDto, Models.Entry>(entryForUpdate);
      entryForCreate.Phonebook = Phonebook;

      await _entriesRepo.AddEntry(entryForCreate);
      var entryForResponse = _mapper.Map<Models.Entry, EntryForResponseDto>(entryForCreate);

      return CreatedAtRoute("GetEntry", new { Id = id, entryid = entryForCreate.Id }, entryForResponse);
    }

    [HttpDelete("phonebooks/{id}/entries/{entryid}")]
    [ServiceFilter(typeof(ValidatePhonebookIdAttribute))]
    [ServiceFilter(typeof(ValidateEntryIdAttribute))]
    public async Task<IActionResult> DeleteEntry(int id, int entryid)
    {
      var entryToDelete = HttpContext.Items[HttpContextConstants.EntryItem] as Models.Entry;
      await _entriesRepo.DeleteEntry(entryToDelete);

      return Ok();
    }

  }
}