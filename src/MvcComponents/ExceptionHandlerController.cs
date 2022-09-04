using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleMvc
{
    [ApiController]
    [Route("[controller]")]
    public class ExceptionHandlerController: ControllerBase
    {
        public ExceptionHandlerController()
        {

        }

        public void Invoke([FromServices] IHttpContextAccessor httpContextAccessor)        
            => this.OnRequest(httpContextAccessor.HttpContext);

        private void OnRequest(HttpContext httpContext)
        {
            
        }
    }
}
