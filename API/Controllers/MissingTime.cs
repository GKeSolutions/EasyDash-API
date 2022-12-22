using Core.Interface;
using Core.Model.MissingTime;
using Microsoft.AspNetCore.Mvc;

namespace EasyDash_API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MissingTime : ControllerBase
    {
        private IMissingTimeService MissingTimeService { get; set; }
        public MissingTime(IMissingTimeService missingTimeService)
        {
            MissingTimeService = missingTimeService;
        }

        [HttpGet]
        public async Task<IEnumerable<Time>> Get()
        {
            return await MissingTimeService.GetMissingTime();
        }
    }
}
