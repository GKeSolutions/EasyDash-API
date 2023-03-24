using Core.Interface;
using Core.Model.Dashboard;
using Core.Model.Dashboard.Process;
using EasyDash_API.Controllers;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[Action]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class Dashboard : BaseController
    {
        private IProcessService ProcessService { get; set; }
        private ICancelService CancelService { get; set; }
        private readonly ILogger<Dashboard> Logger;

        public Dashboard(IProcessService processService, ICancelService cancelService, IHttpContextAccessor httpContextAccessor, ILookupService lookupService, ILogger<Dashboard> logger):base(lookupService, httpContextAccessor)
        {
            ProcessService = processService;
            CancelService = cancelService;
            Logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Core.Model.Dashboard.User.DashUser>> user()
        {
            return await ProcessService.GetUsers();
        }

        [HttpGet]
        public async Task<IEnumerable<DashProcess>> process()
        {
            return await ProcessService.GetProcesses();
        }

        [HttpPost]
        public async Task<IActionResult> Cancel([FromBody] Cancel model)
        {
            await CancelService.CancelProcess(model.ProcessCode, model.ProcItemId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CancelAll([FromBody] Cancel model)
        {
            await CancelService.CancelAll(model);
            return Ok();
        }
    }
}
