using Core.Interface;
using Core.Model.Reassign;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
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
