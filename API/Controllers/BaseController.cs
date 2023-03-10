using Core.Interface;
using Core.Service;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Web.Http;

namespace EasyDash_API.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly ILookupService LookupService;

        public BaseController(ILookupService lookupService, IHttpContextAccessor httpContextAccessor) 
        {
            HttpContextAccessor = httpContextAccessor;
            LookupService = lookupService;
            ValidateUser();
        }

        public string UserName { get { return HttpContextAccessor.HttpContext.User.Identity.Name; }  set { } }

        public void ValidateUser() {
            var valid = LookupService.GetIsActive3EUser(UserName);
            if (!valid) throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }
    }
}
