using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IHttpContextAccessor HttpContextAccessor;

        public BaseController(IHttpContextAccessor httpContextAccessor) 
        {
            HttpContextAccessor = httpContextAccessor;
        }

        public string UserName { get { return HttpContextAccessor.HttpContext.User.Identity.Name; }  set { } }
    }
}
