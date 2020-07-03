using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Phonebook.API.Constants;
using Phonebook.API.Data;
using Phonebook.API.Dtos;
using Models = Phonebook.API.Models;

namespace phonebook.API.Tests.Controller
{
  [TestFixture]
  public class PhoneBooksControllerTests
  {
    private Mock<IPhonebookRepository> mockRepo;
    private Mock<IMapper> mockMapper;

    [SetUp]
    public void Setup()
    {
      mockRepo = new Mock<IPhonebookRepository>();
      mockMapper = new Mock<IMapper>();
    }

    [Test]
    public async Task GetsPhonebook_From_HttpContextCache()
    {
      var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
      var items = new Mock<IDictionary<object, object>>();
      httpContext.SetupGet(hc => hc.Items).Returns(items.Object);
      var controller = new PhoneBooksController(mockRepo.Object, mockMapper.Object);
      controller.ControllerContext = new ControllerContext
      {
        HttpContext = httpContext.Object
      };

      var testPhonebook = new Models.Phonebook { Id = 1, Name = "TestPhonebook" };
      items.SetupGet(i => i[HttpContextConstants.PhonebookItem]).Returns(testPhonebook);

      var phonebookForResponse = new PhonebookForResponseDto { Id = 1, PhonebookName = "TestPhonebook" };
      mockMapper.Setup(mm => mm.Map<PhonebookForResponseDto>(testPhonebook)).Returns(phonebookForResponse);

      var result = await controller.GetPhonebook(1);

      Assert.That(result, Is.InstanceOf<OkObjectResult>());
      var expectedResult = result as OkObjectResult;
      Assert.That(expectedResult.Value, Is.EqualTo(phonebookForResponse));
    }

    [Test]
    public async Task GetsPhonebook_From_Repo_When_ThereISNothingInHttpContextCache()
    {
      var controller = new PhoneBooksController(mockRepo.Object, mockMapper.Object);
      var testPhonebook = new Models.Phonebook { Id = 1, Name = "TestPhonebook" };
      mockRepo.Setup(mr => mr.GetPhonebook(1)).Returns(Task.FromResult(testPhonebook));

      var phonebookForResponse = new PhonebookForResponseDto { Id = 1, PhonebookName = "TestPhonebook" };
      mockMapper.Setup(mm => mm.Map<PhonebookForResponseDto>(testPhonebook)).Returns(phonebookForResponse);

      var result = await controller.GetPhonebook(1);

      Assert.That(result, Is.InstanceOf<OkObjectResult>());
      var expectedResult = result as OkObjectResult;
      Assert.That(expectedResult.Value, Is.EqualTo(phonebookForResponse));
    }

    [Test]
    public async Task UpdatesPhonebook_In_Repo()
    {
      var controller = new PhoneBooksController(mockRepo.Object, mockMapper.Object);
      var testPhonebook = new Models.Phonebook { Id = 1, Name = "TestPhonebook" };
      mockRepo.Setup(mr => mr.GetPhonebook(1)).Returns(Task.FromResult(testPhonebook));
      var testPhonebookForUpdate = new PhonebookForUpdateDto { Name = "NewTestName" };

      var result = await controller.UpdatePhoneBook(1, testPhonebookForUpdate);

      mockRepo.Verify(mr => mr.SaveAsync());
      Assert.That(testPhonebook.Name, Is.EqualTo("NewTestName"));
      Assert.That(result, Is.InstanceOf<OkResult>());
    }
  }
}