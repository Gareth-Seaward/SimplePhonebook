using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Phonebook.API.ActionFilters;
using Phonebook.API.Data;
using Microsoft.AspNetCore.Mvc;
using Models = Phonebook.API.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Phonebook.API.Constants;

namespace phonebook.API.Tests.ActionFilters
{
  [TestFixture]
  public class ValidatePhonebookIdAttributeTests : MockActionExecutingContext
  {
    private Mock<IPhonebookRepository> mockponeRepo;
    private Mock<ActionExecutionDelegate> mockDelegate;

    [SetUp]
    public void Setup()
    {
      mockponeRepo = new Mock<IPhonebookRepository>();
      mockDelegate = new Mock<ActionExecutionDelegate>();
    }

    [Test]
    public async Task RunsNextDelegate_On_ValidPhonebookId()
    {
      var testPhonebookId = 1;
      var actionArgs = new Dictionary<string, object>();
      actionArgs.Add("id", testPhonebookId);

      var testContext = GetActionExecutionContextMock(actionArgs, new Mock<Microsoft.AspNetCore.Mvc.Controller>().Object);

      var testUser = new Models.User { Id = 1 };
      var testPhonebook = new Models.Phonebook { Id = 2, User = testUser };
      mockponeRepo.Setup(mpr => mpr.GetPhonebook(testPhonebookId)).Returns(Task.FromResult(testPhonebook));

      var filter = new ValidatePhonebookIdAttribute(mockponeRepo.Object);
      await filter.OnActionExecutionAsync(testContext, mockDelegate.Object);

      mockDelegate.Verify(md => md.Invoke());
      var expectedCachedResult = testContext.HttpContext.Items[HttpContextConstants.PhonebookItem] as Models.Phonebook;
      Assert.That(expectedCachedResult, Is.EqualTo(testPhonebook));
    }

    [Test]
    public async Task Returns_Unathorised_ForClaimNotMatchingPhonebook()
    {
      var testPhonebookId = 1;
      var actionArgs = new Dictionary<string, object>();
      actionArgs.Add("id", testPhonebookId);

      var testContext = GetActionExecutionContextMock(actionArgs, new Mock<Microsoft.AspNetCore.Mvc.Controller>().Object);

      var testUser = new Models.User { Id = 2 };
      var testPhonebook = new Models.Phonebook { Id = 2, User = testUser };
      mockponeRepo.Setup(mpr => mpr.GetPhonebook(testPhonebookId)).Returns(Task.FromResult(testPhonebook));

      var filter = new ValidatePhonebookIdAttribute(mockponeRepo.Object);
      await filter.OnActionExecutionAsync(testContext, mockDelegate.Object);

      Assert.That(testContext.Result, Is.InstanceOf<UnauthorizedResult>());
    }

    [Test]
    public async Task Returns_BadRequest_ForMissingIdParameter()
    {
      var actionArgs = new Dictionary<string, object>();

      var testContext = GetActionExecutionContextMock(actionArgs, new Mock<Microsoft.AspNetCore.Mvc.Controller>().Object);

      var filter = new ValidatePhonebookIdAttribute(mockponeRepo.Object);
      await filter.OnActionExecutionAsync(testContext, mockDelegate.Object);

      Assert.That(testContext.Result, Is.InstanceOf<BadRequestObjectResult>());
      var badRequestResult = testContext.Result as BadRequestObjectResult;
      Assert.That(badRequestResult.Value, Is.EqualTo("Bad input parameter"));
    }
  }
}