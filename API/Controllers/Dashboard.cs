using Core.Interface;
using Core.Model.Dashboard.Process;
using Core.Model.Dashboard.User;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
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
        public async Task<IEnumerable<User>> user()
        {
            return await ProcessService.GetUsers();
        }

        [HttpGet]
        public async Task<IEnumerable<Process>> process()
        {
            return await ProcessService.GetProcesses();
        }
    }
}
