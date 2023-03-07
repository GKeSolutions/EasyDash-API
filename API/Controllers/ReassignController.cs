using Core.Interface;
using Core.Model.Reassign;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class ReassignController : ControllerBase
    {
        private readonly IReassignService ReassignService;
        public ReassignController(IReassignService reassignService)
        {
            ReassignService= reassignService;
        }

        [HttpPost]
        public async Task<string> Reassign([FromBody] ReassignModel model)
        {
            return await ReassignService.Reassign(model.ProcessCode, model.ProcItemId, model.ReassignToUserId);
        }
    }
}
