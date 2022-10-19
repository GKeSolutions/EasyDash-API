using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class Lookup : ControllerBase
    {
        private ILookupService  LookupService{ get; set; }

        public Lookup(ILookupService lookupService)
        {
            LookupService = lookupService;
        }

        [HttpGet]
        public async Task<object> Roles()
        {
            return await LookupService.GetRoles();
        }
    }
}
