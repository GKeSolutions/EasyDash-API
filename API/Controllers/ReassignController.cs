using Core.Interface;
using Core.Model.Reassign;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ReassignController : BaseController
    {
        private readonly IReassignService ReassignService;
        public ReassignController(IReassignService reassignService, IHttpContextAccessor httpContextAccessor):base(httpContextAccessor)
        {
            ReassignService= reassignService;
        }

        [HttpPost]
        public async Task<IActionResult> Reassign([FromBody] ReassignModel model)
        {
            await ReassignService.Reassign(model.ProcessCode, model.ProcItemId, model.ReassignToUserId, UserName);
            return Ok();
        }
    }
}
