using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using ReadEdgeCore.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GleamTech.DocumentUltimateExamples.AspNetCoreCS.Filters
{
    public class AsyncActionFilter : IAsyncActionFilter
    {

        public AsyncActionFilter()
        {
  
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
        // execute any code before the action executes
           var login = context.HttpContext.Session.GetString("UserId");

            if (login == "" || login ==null)
            {
             context.Result = new RedirectToRouteResult(
             new RouteValueDictionary
             {
                     { "controller", "Account" },
                     { "action", "Login" }
             });
            }
            else
            {
                var result = await next();
            }
        }
    }
}
