using Core.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class Lookup : BaseController
    {
        private ILookupService  LookupService{ get; set; }

        public Lookup(ILookupService lookupService, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            LookupService = lookupService;
        }

        [HttpGet]
        public async Task<object> Roles()
        {
            return await LookupService.GetRoles();
        }

        [HttpGet]
        public async Task<object> UsersRoles()
        {
            return await LookupService.GetUsersRoles();
        }
    }
}
