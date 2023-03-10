using Core.Interface;
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
        private IDashboardService ProcessService { get; set; }
        private readonly ILogger<Dashboard> Logger;

        public Dashboard(IDashboardService processService, IHttpContextAccessor httpContextAccessor, ILookupService lookupService, ILogger<Dashboard> logger):base(lookupService, httpContextAccessor)
        {
            ProcessService = processService;
            Logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Core.Model.Dashboard.User.DashUser>> user()
        {
            Logger.LogError("The windows username:" + UserName);
            return await ProcessService.GetUsers();
        }

        [HttpGet]
        public async Task<IEnumerable<DashProcess>> process()
        {
            return await ProcessService.GetProcesses();
        }
    }
}
