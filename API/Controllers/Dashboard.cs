using Core.Interface;
using Core.Model.Dashboard.Process;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[Action]")]
    [EnableCors("_myAllowSpecificOrigins")]
    public class Dashboard : ControllerBase
    {
        private IDashboardService ProcessService { get; set; }
        private readonly IHttpContextAccessor HttpContextAccessor;
        private readonly ILogger<Dashboard> Logger;

        public Dashboard(IDashboardService processService, IHttpContextAccessor httpContextAccessor, ILogger<Dashboard> logger)
        {
            ProcessService = processService;
            HttpContextAccessor = httpContextAccessor;
            Logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Core.Model.Dashboard.User.DashUser>> user()
        {
            Logger.LogError("The windows username:" + HttpContextAccessor.HttpContext.User.Identity.Name);
            return await ProcessService.GetUsers();
        }

        [HttpGet]
        public async Task<IEnumerable<DashProcess>> process()
        {
            return await ProcessService.GetProcesses();
        }
    }
}
