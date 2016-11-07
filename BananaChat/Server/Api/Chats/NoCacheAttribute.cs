using System.Net.Http.Headers;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace Server.Api.Chats
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext?.Response == null) return;

            actionContext.Response.Headers.CacheControl = new CacheControlHeaderValue
                                                          {
                                                              NoCache = true
                                                          };
        }
    }
}