using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Phonebook.API.Constants;
using Phonebook.API.Controllers;
using Phonebook.API.Data;
using Phonebook.API.Dtos;
using Phonebook.API.Helpers;
using Phonebook.API.Models;

namespace phonebook.API.Tests.Controller
{
  [TestFixture]
  public class EntriesControllerTests
  {
    private Mock<IEntriesRepository> mockRepo;
    private Mock<IMapper> mockMapper;

    [SetUp]
    public void Setup()
    {
      mockRepo = new Mock<IEntriesRepository>();
      mockMapper = new Mock<IMapper>();
    }

    [Test]
    public async Task Get_Entries_For_Phonebook()
    {
      var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
      var response = new Mock<HttpResponse>();
      response.SetupProperty(it => it.StatusCode);
      var headers = new Mock<IHeaderDictionary>();
      response.SetupGet(it => it.Headers).Returns(headers.Object);
      httpContext.Setup(hc => hc.Response).Returns(response.Object);

      var controller = new EntriesController(mockRepo.Object, mockMapper.Object);
      controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext.Object
      };

      var testPhonebookId = 1;
      var testParams = new EntryParams();

      var testPagedEntries = GetTestPagedEntries();
      mockRepo.Setup(mr => mr.GetEntries(testPhonebookId, testParams)).Returns(Task.FromResult(testPagedEntries));

      var testResponseEntries = GetTestResponseEntries();
      mockMapper.Setup(mm => mm.Map<IEnumerable<EntryForResponseDto>>(testPagedEntries)).Returns(testResponseEntries);
      var result = await controller.GetEntries(testPhonebookId, testParams);

      Assert.That(result, Is.InstanceOf<OkObjectResult>());
    }

    [Test]
    public async Task GetsEntryFromHttpContextCache_For_EntryId()
    {
      var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
      var items = new Mock<IDictionary<object, object>>();
      httpContext.SetupGet(hc => hc.Items).Returns(items.Object);
      var controller = new EntriesController(mockRepo.Object, mockMapper.Object);
      controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext.Object
      };

      var testEntry = new Entry { Id = 1 };
      items.SetupGet(i => i[HttpContextConstants.EntryItem]).Returns(testEntry);

      var testResponseEntry = new EntryForResponseDto { Id = 1 };
      mockMapper.Setup(mm => mm.Map<Entry, EntryForResponseDto>(testEntry)).Returns(testResponseEntry);

      var result = await controller.GetEntry(1, 1);

      Assert.That((result as OkObjectResult).Value, Is.EqualTo(testResponseEntry));
    }

    private PagedList<Entry> GetTestPagedEntries()
    {
      var entries = new List<Entry>
          {
            new Entry{Id = 1},
            new Entry{Id = 2},
            new Entry{Id = 3},
          };
      return new PagedList<Entry>(entries, 3, 1, 5);
    }

    private IEnumerable<EntryForResponseDto> GetTestResponseEntries()
    {
      return new List<EntryForResponseDto>
          {
            new EntryForResponseDto{Id = 1},
            new EntryForResponseDto {Id = 2},
            new EntryForResponseDto {Id = 3}
          };
    }
  }
}