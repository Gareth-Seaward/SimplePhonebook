using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using Phonebook.API.ActionFilters;
using Phonebook.API.Constants;
using Phonebook.API.Data;
using Models = Phonebook.API.Models;

namespace phonebook.API.Tests.ActionFilters
{
  [TestFixture]
  public class ValidateEntryIdAttributeTests : MockActionExecutingContext
  {
    private Mock<IEntriesRepository> mockEntriesRepo;
    private Mock<ActionExecutionDelegate> mockDelegate;

    [SetUp]
    public void Setup()
    {
      mockEntriesRepo = new Mock<IEntriesRepository>();
      mockDelegate = new Mock<ActionExecutionDelegate>();
    }

    [Test]
    public async Task RunsNextDelegate_For_MatchingEntryPhonebookCombination()
    {
      var testEntryId = 1;
      var actionArgs = new Dictionary<string, object>();
      actionArgs.Add("entryid", testEntryId);

      var testContext = GetActionExecutionContextMock(actionArgs, new Mock<Microsoft.AspNetCore.Mvc.Controller>().Object);
      var testPhonebook = new Models.Phonebook { Id = 2 };
      testContext.HttpContext.Items[HttpContextConstants.PhonebookItem] = testPhonebook;

      var testEntry = new Models.Entry { Id = 3, Phonebook = testPhonebook };
      mockEntriesRepo.Setup(mer => mer.GetEntry(1)).Returns(Task.FromResult(testEntry));

      var filter = new ValidateEntryIdAttribute(mockEntriesRepo.Object);

      await filter.OnActionExecutionAsync(testContext, mockDelegate.Object);

      mockDelegate.Verify(md => md.Invoke());
    }

    [Test]
    public async Task ReturnsUnauthrisedResult_For_NonmatchingPhonebook()
    {
      var testEntryId = 1;
      var actionArgs = new Dictionary<string, object>();
      actionArgs.Add("entryid", testEntryId);

      var testContext = GetActionExecutionContextMock(actionArgs, new Mock<Microsoft.AspNetCore.Mvc.Controller>().Object);
      var testPhonebook = new Models.Phonebook { Id = 2 };
      testContext.HttpContext.Items[HttpContextConstants.PhonebookItem] = testPhonebook;

      var testMismatchPhonebook = new Models.Phonebook { Id = 3 };
      var testEntry = new Models.Entry { Id = 3, Phonebook = testMismatchPhonebook };
      mockEntriesRepo.Setup(mer => mer.GetEntry(1)).Returns(Task.FromResult(testEntry));

      var filter = new ValidateEntryIdAttribute(mockEntriesRepo.Object);

      await filter.OnActionExecutionAsync(testContext, mockDelegate.Object);

      Assert.That(testContext.Result, Is.InstanceOf<UnauthorizedResult>());
    }

    [Test]
    public async Task ReturnsBadREquest_For_MissingEntryId()
    {

      var actionArgs = new Dictionary<string, object>();
      var testContext = GetActionExecutionContextMock(actionArgs, new Mock<Microsoft.AspNetCore.Mvc.Controller>().Object);
      var testPhonebook = new Models.Phonebook { Id = 2 };
      testContext.HttpContext.Items[HttpContextConstants.PhonebookItem] = testPhonebook;

      var filter = new ValidateEntryIdAttribute(mockEntriesRepo.Object);

      await filter.OnActionExecutionAsync(testContext, mockDelegate.Object);

      Assert.That(testContext.Result, Is.InstanceOf<BadRequestObjectResult>());
      var badRequestResult = testContext.Result as BadRequestObjectResult;
      Assert.That(badRequestResult.Value, Is.EqualTo("Bad input parameter"));
    }
  }
}