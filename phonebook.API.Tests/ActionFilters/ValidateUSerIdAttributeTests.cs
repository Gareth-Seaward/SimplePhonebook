using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;
using NUnit.Framework;
using Phonebook.API.ActionFilters;
using Phonebook.API.Data;

namespace phonebook.API.Tests.ActionFilters
{
  [TestFixture]
  public class ValidateUSerIdAttributeTests : MockActionExecutingContext
  {
    private Mock<ActionExecutionDelegate> mockDelegate;


    [SetUp]
    public void Setup()
    {
      mockDelegate = new Mock<ActionExecutionDelegate>();
    }

    [Test]
    public async Task RunsNextDelegate_OnValidUserId()
    {
      var testUserId = 1;
      var actionArgs = new Dictionary<string, object>();
      actionArgs.Add("id", testUserId);

      var testContext = GetActionExecutionContextMock(actionArgs, new Mock<Microsoft.AspNetCore.Mvc.Controller>().Object);

      var filter = new ValidateUSerIdAttribute();

      await filter.OnActionExecutionAsync(testContext, mockDelegate.Object);

      mockDelegate.Verify(md => md.Invoke());
    }

    [Test]
    public async Task ReturnsUnauthrised_For_InvlaidUserId()
    {
      var testUserId = 2;
      var actionArgs = new Dictionary<string, object>();
      actionArgs.Add("id", testUserId);

      var testContext = GetActionExecutionContextMock(actionArgs, new Mock<Microsoft.AspNetCore.Mvc.Controller>().Object);

      var filter = new ValidateUSerIdAttribute();

      await filter.OnActionExecutionAsync(testContext, mockDelegate.Object);

      Assert.That(testContext.Result, Is.InstanceOf<UnauthorizedResult>());
    }

    [Test]
    public async Task ReturnsBadRequest_For_MissingUserId()
    {
      var actionArgs = new Dictionary<string, object>();

      var testContext = GetActionExecutionContextMock(actionArgs, new Mock<Microsoft.AspNetCore.Mvc.Controller>().Object);

      var filter = new ValidateUSerIdAttribute();

      await filter.OnActionExecutionAsync(testContext, mockDelegate.Object);

      Assert.That(testContext.Result, Is.InstanceOf<BadRequestObjectResult>());
      var badRequestResult = testContext.Result as BadRequestObjectResult;
      Assert.That(badRequestResult.Value, Is.EqualTo("Bad input parameter"));
    }
  }
}