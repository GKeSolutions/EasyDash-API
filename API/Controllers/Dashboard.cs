using Core.Interface;
using Core.Model.Dashboard.Process;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[Action]")]
    public class Dashboard : ControllerBase
    {
        private IDashboardService ProcessService { get; set; }

        public Dashboard(IDashboardService processService)
        {
            ProcessService = processService;
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
    }
}
