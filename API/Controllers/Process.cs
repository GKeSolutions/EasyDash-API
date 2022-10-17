using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]/{id?}")]
    public class Process : ControllerBase
    {
        private IProcessService ProcessService { get; set; }
        public Process(IProcessService processService)
        {
            ProcessService = processService;
        }

        [HttpGet]
        public async Task<object> Get(int id)
        {
            return await ProcessService.GetProcesses(id);
        }
    }
}
