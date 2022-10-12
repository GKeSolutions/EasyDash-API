using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Process : ControllerBase
    {
        private IProcessService ProcessService { get; set; }
        public Process(IProcessService processService)
        {
            ProcessService = processService;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            return await ProcessService.GetProcesses();
        }
    }
}
