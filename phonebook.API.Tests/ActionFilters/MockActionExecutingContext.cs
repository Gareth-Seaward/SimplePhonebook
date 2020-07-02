using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;

namespace phonebook.API.Tests.ActionFilters
{
    public abstract class MockActionExecutingContext : MockClaimPrinciple
    {
      protected ActionExecutingContext GetActionExecutionContextMock(IDictionary<string, object> actionArgs,object controller)
      {
        return new ActionExecutingContext(
          GetActionContext(),
          new List<IFilterMetadata>(), actionArgs, controller);
        
      }

      private ActionContext GetActionContext()
      {
        var httpContext = new DefaultHttpContext();
        httpContext.User = GetClaimsPrincipleMock().Object;
        return new ActionContext 
        { 
          HttpContext = httpContext, 
          RouteData = new RouteData(), 
          ActionDescriptor = new ActionDescriptor()};
      }
        
    }
}